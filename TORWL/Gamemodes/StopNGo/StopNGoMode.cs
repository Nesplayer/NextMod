using System.Collections;
using AmongUs.GameOptions;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using MiraAPI.GameOptions;
using Reactor.Networking.Attributes;
using TORWL.Networking;
using UnityEngine;

namespace TORWL.Features.StopNGo;

/// <summary>
/// Stop 'n Go — a pure survival gamemode.
/// The light alternates between green (move freely) and red (moving = death).
/// Last crewmate standing wins. No impostors.
/// </summary>
public static class StopNGoMode
{
    // ── State ─────────────────────────────────────────────────────────────────

    public static bool IsActive { get; private set; }
    public static bool KillsAllowed { get; private set; } = false;
    public static LightState CurrentLight { get; private set; } = LightState.Green;

    public enum LightState : byte { Green = 0, Red = 1 }

    public const float MovementThreshold = 0.08f;

    private static Coroutine? _lightLoop;

    private static StopNGoOptions Options =>
        OptionGroupSingleton<StopNGoOptions>.Instance;

    // ── Activation ────────────────────────────────────────────────────────────

    public static void Activate()
    {
        if (!AmongUsClient.Instance.AmHost) return;
        IsActive = true;
        CurrentLight = LightState.Green;
        PlayerControl.LocalPlayer.RpcSetStopNGoActive(true);
        _lightLoop = AmongUsClient.Instance.StartCoroutine(LightLoop().WrapToIl2Cpp());
    }

    public static void Deactivate()
    {
        IsActive = false;
        if (_lightLoop != null)
        {
            AmongUsClient.Instance.StopCoroutine(_lightLoop);
            _lightLoop = null;
        }
        if (AmongUsClient.Instance.AmHost)
            PlayerControl.LocalPlayer.RpcSetStopNGoActive(false);
    }

    // ── Light loop (host only) ────────────────────────────────────────────────

    private static IEnumerator LightLoop()
    {
        while (IsActive)
        {
            yield return RunPhase(LightState.Green, Options.GreenDuration);
            if (!IsActive) yield break;
            yield return RunPhase(LightState.Red, Options.RedDuration);
        }
    }

    private static IEnumerator RunPhase(LightState state, float duration)
    {
        SetLight(state);
        KillsAllowed = false;

        // Clear stale positions so players aren't killed instantly for movement
        // that happened during the previous green phase.
        if (state == LightState.Red)
            StopNGoPatches.ClearPositions();

        var remaining = duration;
        while (remaining > 0f)
        {
            // Only allow kills after red light is established and not in the final window
            KillsAllowed = state == LightState.Red && remaining > 0.5f;
            PlayerControl.LocalPlayer.RpcSetStopNGoTimer(remaining);
            yield return new WaitForSeconds(1f);
            remaining -= 1f;
            if (!IsActive) yield break;
        }
        KillsAllowed = false;
    }

    private static void SetLight(LightState state)
    {
        CurrentLight = state;
        PlayerControl.LocalPlayer.RpcSetStopNGoLight((byte)state);
    }

    // ── Win condition check (host only) ──────────────────────────────────────

    public static void CheckWinCondition()
    {
        if (!IsActive || !AmongUsClient.Instance.AmHost) return;

        int alive = 0;
        foreach (var pc in PlayerControl.AllPlayerControls)
        {
            if (pc == null || pc.Data == null) continue;
            if (!pc.Data.IsDead) alive++;
        }

        if (alive <= 1)
        {
            Deactivate();
            GameManager.Instance?.RpcEndGame((GameOverReason)CustomGameOverReason.StopNGoWin, false);
        }
    }

    // ── RPCs ──────────────────────────────────────────────────────────────────

    [MethodRpc((uint)LaunchpadRpc.StopNGoSetActive)]
    public static void RpcSetStopNGoActive(this PlayerControl sender, bool active)
    {
        IsActive = active;
        if (!active) CurrentLight = LightState.Green;
        StopNGoHud.SetVisible(active);
    }

    [MethodRpc((uint)LaunchpadRpc.StopNGoSetLight)]
    public static void RpcSetStopNGoLight(this PlayerControl sender, byte state)
    {
        CurrentLight = (LightState)state;
        StopNGoHud.UpdateLight(CurrentLight);
    }

    [MethodRpc((uint)LaunchpadRpc.StopNGoSetTimer)]
    public static void RpcSetStopNGoTimer(this PlayerControl sender, float secondsLeft)
    {
        StopNGoHud.UpdateTimer(secondsLeft);
    }
}

public enum CustomGameOverReason
{
    StopNGoWin = 10,
}