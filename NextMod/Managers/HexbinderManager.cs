using System;
using System.Collections.Generic;
using System.Collections;
using HarmonyLib;
using MiraAPI.GameOptions;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TMPro;
using NEXT.Options.Roles.Coven;
using NEXT.Roles.Coven;
using UnityEngine;

namespace NEXT.Managers;

public static class HexbinderManager
{
    private static readonly Dictionary<byte, int> _hexedPlayers = new();
    private const string HexLabelTag = "HexLabel";
    private const string HexUnicode = "乂";

    public static void HexPlayer(PlayerControl target)
    {
        var meetings = (int)OptionGroupSingleton<HexbinderOptions>.Instance.MeetingsUntilDeath;
        _hexedPlayers[target.PlayerId] = meetings;
        ShowHexLabel(target, meetings);
    }

    public static bool IsHexed(byte playerId) => _hexedPlayers.ContainsKey(playerId);
    public static IReadOnlyDictionary<byte, int> HexedPlayers => _hexedPlayers;

    public static void Reset()
    {
        foreach (var playerId in _hexedPlayers.Keys)
        {
            var player = GameData.Instance.GetPlayerById(playerId)?.Object;
            if (player != null) ClearHexLabel(player);
        }
        _hexedPlayers.Clear();
    }

    private static void ShowHexLabel(PlayerControl player, int meetingsLeft)
    {
        if (player == null) return;

        // Only show to the target or the local hexbinder
        if (player != PlayerControl.LocalPlayer &&
            PlayerControl.LocalPlayer?.Data?.Role is not HexbinderRole) return;

        if (player?.cosmetics?.nameTextContainer == null) return;

        ClearHexLabel(player);

        var go = new GameObject(HexLabelTag);
        go.transform.SetParent(player.cosmetics.nameTextContainer.transform, false);
        go.transform.localPosition = new Vector3(0f, 0.35f, -1f);

        var tmp = go.AddComponent<TextMeshPro>();
        string meetingWord = meetingsLeft == 1 ? "meeting" : "meetings";
        tmp.text = $"<color=#8B0000><b>Dies after {meetingsLeft} {meetingWord}!</b></color>";
        tmp.fontSize = 1.8f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.richText = true;
    }

    private static void ClearHexLabel(PlayerControl player)
    {
        var t = player?.cosmetics?.nameTextContainer?.transform.FindChild(HexLabelTag);
        if (t != null) t.gameObject.Destroy();
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public static class MeetingStartPatch
    {
        public static void Postfix(MeetingHud __instance)
        {
            var toKill = new List<byte>();

            foreach (var (playerId, meetingsLeft) in _hexedPlayers)
            {
                var player = GameData.Instance.GetPlayerById(playerId)?.Object;
                if (player != null) ClearHexLabel(player);

                var newCount = meetingsLeft - 1;
                if (newCount <= 0)
                {
                    toKill.Add(playerId);
                }
                else
                {
                    _hexedPlayers[playerId] = newCount;

                    // Add unicode indicator in meeting for everyone
                    foreach (var pv in __instance.playerStates)
                    {
                        if (pv.TargetPlayerId != playerId) continue;
                        pv.NameText.text += $" <color=#f533ff>{HexUnicode}</color>";
                        break;
                    }
                }
            }

            foreach (var playerId in toKill)
            {
                _hexedPlayers.Remove(playerId);

                // Add unicode indicator in meeting for everyone
                foreach (var pv in __instance.playerStates)
                {
                    if (pv.TargetPlayerId != playerId) continue;
                    pv.NameText.text += $" <color=#8B0000>{HexUnicode}</color>";
                    break;
                }
            }

            Coroutines.Start(KillAfterMeeting(toKill));
        }

        private static bool MeetingEnded() => MeetingHud.Instance == null;

        private static IEnumerator KillAfterMeeting(List<byte> toKill)
        {
            yield return new WaitUntil((Func<bool>)MeetingEnded);

            foreach (var playerId in toKill)
            {
                var player = GameData.Instance.GetPlayerById(playerId)?.Object;
                if (player == null || player.Data.IsDead) continue;
                player.Exiled();
            }
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Close))]
    public static class MeetingClosePatch
    {
        public static void Postfix()
        {
            foreach (var (playerId, meetingsLeft) in _hexedPlayers)
            {
                var player = GameData.Instance.GetPlayerById(playerId)?.Object;
                if (player != null) ShowHexLabel(player, meetingsLeft);
            }
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
    public static class GameStartPatch
    {
        public static void Postfix() => Reset();
    }
}