using HarmonyLib;
using TMPro;
using UnityEngine;
using MiraAPI.GameOptions;
using TORWL.Options;
using Reactor.Networking.Attributes;
using Reactor.Networking.Rpc;
using Hazel;
using TORWL.Networking;

namespace TORWL.Patches.Generic;

[HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.Update))]
public static class AutoStartPatch
{
    private static float _timer = -1f;
    private static TextMeshPro? _countdownText;
    private static float _rpcSendInterval = 0f;

    // Written by host via RPC, read locally by everyone including host
    public static float SharedSecondsRemaining = -1f;
    public static bool SharedWaitingForPlayers = false;
    public static int SharedPlayersNeeded = 0;
    public static bool SharedAutoStartActive = false;

    public static void Postfix(GameStartManager __instance)
    {
        if (_countdownText == null)
            _countdownText = CreateCountdownLabel(__instance);

        if (AmongUsClient.Instance.AmHost)
        {
            var opts = OptionGroupSingleton<GeneralOptions>.Instance;
            int playerCount = GameData.Instance != null ? GameData.Instance.PlayerCount : 0;
            bool meetsMinimum = playerCount >= (int)opts.AutoStartMinPlayers;

            if (!opts.AutoStart || !meetsMinimum)
            {
                _timer = opts.AutoStartAfter;
                SharedAutoStartActive = opts.AutoStart;
                SharedWaitingForPlayers = opts.AutoStart && !meetsMinimum;
                SharedPlayersNeeded = (int)opts.AutoStartMinPlayers - playerCount;
                SharedSecondsRemaining = -1f;
            }
            else
            {
                _timer -= Time.deltaTime;
                SharedAutoStartActive = true;
                SharedWaitingForPlayers = false;
                SharedSecondsRemaining = _timer;

                if (_timer <= 0f)
                {
                    _timer = opts.AutoStartAfter;
                    SharedSecondsRemaining = -1f;
                    __instance.BeginGame();
                }
            }

            // Send RPC to clients every 0.5s so they stay in sync
            _rpcSendInterval -= Time.deltaTime;
            if (_rpcSendInterval <= 0f)
            {
                _rpcSendInterval = 0.5f;
                AutoStartSyncRpc.Send(
                    SharedAutoStartActive,
                    SharedWaitingForPlayers,
                    SharedPlayersNeeded,
                    SharedSecondsRemaining
                );
            }
        }

        // Everyone (host + clients) draws the label from shared state
        UpdateLabel();
    }

    private static void UpdateLabel()
    {
        if (_countdownText == null) return;

        if (!SharedAutoStartActive)
        {
            _countdownText.text = "";
        }
        else if (SharedWaitingForPlayers)
        {
            _countdownText.text = $"<color=#888888>Waiting for {SharedPlayersNeeded} more player(s)...</color>";
        }
        else if (SharedSecondsRemaining >= 0f)
        {
            int seconds = Mathf.CeilToInt(SharedSecondsRemaining);
            string color = seconds > 15 ? "#15ff34" : seconds > 5 ? "#ffd117" : "#ff0f0f";
            _countdownText.text = $"<color={color}>Auto-starting in {seconds}s...</color>";
        }
        else
        {
            _countdownText.text = "";
        }
    }

    private static TextMeshPro CreateCountdownLabel(GameStartManager manager)
    {
        var startBtn = manager.StartButton?.transform;
        if (startBtn == null) return null!;

        var obj = new GameObject("AutoStartCountdown");
        obj.transform.SetParent(startBtn.parent, false);
        obj.transform.localPosition = new Vector3(-0.1774f, -0.4264f, 0f);

        var tmp = obj.AddComponent<TextMeshPro>();
        tmp.fontSize = 2f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.text = "";

        return tmp;
    }

    [HarmonyPatch(typeof(GameStartManager), nameof(GameStartManager.OnDestroy))]
    public static class ResetPatch
    {
        public static void Postfix()
        {
            _timer = -1f;
            _countdownText = null;
            SharedSecondsRemaining = -1f;
            SharedWaitingForPlayers = false;
            SharedAutoStartActive = false;
            _rpcSendInterval = 0f;
        }
    }
}

// Reactor custom RPC — host sends countdown state to all clients
[RegisterCustomRpc((uint)LaunchpadRpc.AutoStartSync)]
public class AutoStartSyncRpc : PlayerCustomRpc<TORWLPlugin, AutoStartSyncRpc.Data>
{

    public override RpcLocalHandling LocalHandling => RpcLocalHandling.None;

    public AutoStartSyncRpc(TORWLPlugin plugin, uint id) : base(plugin, id) { }

    public record Data(bool Active, bool Waiting, int PlayersNeeded, float SecondsRemaining);

    public static void Send(bool active, bool waiting, int playersNeeded, float seconds)
    {
        Rpc<AutoStartSyncRpc>.Instance.Send(new Data(active, waiting, playersNeeded, seconds));
    }

    public override void Write(MessageWriter writer, Data data)
    {
        writer.Write(data.Active);
        writer.Write(data.Waiting);
        writer.Write(data.PlayersNeeded);
        writer.Write(data.SecondsRemaining);
    }

    public override Data Read(MessageReader reader)
    {
        return new Data(
            reader.ReadBoolean(),
            reader.ReadBoolean(),
            reader.ReadInt32(),
            reader.ReadSingle()
        );
    }

    public override void Handle(PlayerControl innerNetObject, Data data)
    {
        // Update shared state on the receiving client
        AutoStartPatch.SharedAutoStartActive = data.Active;
        AutoStartPatch.SharedWaitingForPlayers = data.Waiting;
        AutoStartPatch.SharedPlayersNeeded = data.PlayersNeeded;
        AutoStartPatch.SharedSecondsRemaining = data.SecondsRemaining;
    }
}