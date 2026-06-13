using HarmonyLib;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;
using TMPro;
using TORWL.Utilities;
using UnityEngine;
using System.Collections;

namespace TORWL.Managers;

public static class PoisonerManager
{
    public static byte? PoisonedPlayer { get; private set; }
    private static byte? _pendingKill;

    private const string PoisonLabelTag = "PoisonLabel";
    private const string PoisonUnicode = "<color=#34be40>⦿</color>";

    public static void PoisonPlayer(PlayerControl target)
    {
        PoisonedPlayer = target.PlayerId;
        ShowPoisonLabel(target);
    }

    public static void CurePlayer()
    {
        if (PoisonedPlayer != null)
        {
            var player = GameData.Instance.GetPlayerById(PoisonedPlayer.Value)?.Object;
            if (player != null) ClearPoisonLabel(player);
        }
        PoisonedPlayer = null;
    }

    public static void Reset()
    {
        if (PoisonedPlayer != null)
        {
            var player = GameData.Instance.GetPlayerById(PoisonedPlayer.Value)?.Object;
            if (player != null) ClearPoisonLabel(player);
        }
        PoisonedPlayer = null;
        _pendingKill = null;
    }

    private static void ShowPoisonLabel(PlayerControl player)
    {
        if (player?.cosmetics?.nameTextContainer == null) return;

        ClearPoisonLabel(player);

        var go = new GameObject(PoisonLabelTag);
        go.transform.SetParent(player.cosmetics.nameTextContainer.transform, false);
        go.transform.localPosition = new Vector3(0f, 0.35f, -1f);

        var tmp = go.AddComponent<TextMeshPro>();
        tmp.text = "<color=#34be40><b>Dies at the beginning of\nthe next meeting</b></color>";
        tmp.fontSize = 1.8f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.richText = true;
    }

    private static void ClearPoisonLabel(PlayerControl player)
    {
        var t = player.cosmetics.nameTextContainer.transform.FindChild(PoisonLabelTag);
        if (t != null) t.gameObject.Destroy();
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public static class MeetingStartPatch
    {
        public static void Postfix(MeetingHud __instance)
        {
            if (PoisonedPlayer == null) return;

            // Clear in-game label during meeting
            var player = GameData.Instance.GetPlayerById(PoisonedPlayer.Value)?.Object;
            if (player != null) ClearPoisonLabel(player);

            // Add unicode indicator next to name in meeting for everyone
            foreach (var pv in __instance.playerStates)
            {
                if (pv.TargetPlayerId != PoisonedPlayer.Value) continue;
                pv.NameText.text += $" {PoisonUnicode}";
                break;
            }

            _pendingKill = PoisonedPlayer.Value;
            PoisonedPlayer = null;
        }
    }

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.VotingComplete))]
    public static class VotingCompletePatch
    {
        public static void Postfix()
        {
            if (_pendingKill == null) return;

            var target = GameData.Instance.GetPlayerById(_pendingKill.Value)?.Object;
            _pendingKill = null;

            if (target == null || target.Data.IsDead) return;
            target.Exiled();
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
    public static class GameStartPatch
    {
        public static void Postfix() => Reset();
    }
}