using AmongUs.GameOptions;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using MiraAPI.GameOptions;
using MiraAPI.Networking;
using TORWL.Options;
using UnityEngine;

namespace TORWL.Features.StopNGo;

[HarmonyPatch]
public static class StopNGoPatches
{
    private static bool IsStopNGoEnabled =>
        OptionGroupSingleton<GeneralOptions>.Instance.Gamemode.Value == (int)LaunchpadGamemode.StopNGo;

    // ── Game start ────────────────────────────────────────────────────────────

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.StartGame))]
    [HarmonyPostfix]
    public static void StartGame_Postfix()
    {
        if (!AmongUsClient.Instance.AmHost) return;
        if (!IsStopNGoEnabled) return;
        StopNGoMode.Activate();
    }

    // ── Force all crewmate roles ──────────────────────────────────────────────

    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    [HarmonyPrefix]
    public static bool SelectRoles_Prefix()
    {
        if (!StopNGoMode.IsActive) return true;
        foreach (var pc in PlayerControl.AllPlayerControls)
        {
            if (pc == null) continue;
            pc.RpcSetRole(RoleTypes.Crewmate, true);
        }
        return false;
    }

    // ── Movement detection (red light) ────────────────────────────────────────

    private static readonly System.Collections.Generic.Dictionary<int, Vector2> _lastPos = new();

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
    [HarmonyPostfix]
    public static void FixedUpdate_Postfix(PlayerControl __instance)
    {
        if (!StopNGoMode.IsActive) return;
        if (StopNGoMode.CurrentLight != StopNGoMode.LightState.Red) return;
        if (!StopNGoMode.KillsAllowed) return;
        if (!AmongUsClient.Instance.AmHost) return;
        if (__instance.Data == null || __instance.Data.IsDead) return;

        var id = __instance.PlayerId;
        var pos = (Vector2)__instance.transform.position;

        if (_lastPos.TryGetValue(id, out var last))
        {
            if (Vector2.Distance(pos, last) > StopNGoMode.MovementThreshold)
            {
                PlayerControl.LocalPlayer.RpcCustomMurder(
                    __instance,
                    resetKillTimer: false,
                    teleportMurderer: false,
                    showKillAnim: true,
                    playKillSound: true
                );
                _lastPos[id] = pos;
                StopNGoMode.CheckWinCondition();
                return;
            }
        }

        _lastPos[id] = pos;
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
    [HarmonyPostfix]
    public static void GameManagerStart_Postfix()
    {
        _lastPos.Clear();
    }

    /// <summary>
    /// Clears stale position snapshots so players aren't killed on the first
    /// FixedUpdate tick of a new red phase for movement that happened during green.
    /// </summary>
    public static void ClearPositions() => _lastPos.Clear();

    // ── Freeplay support ──────────────────────────────────────────────────────

    [HarmonyPatch(typeof(TutorialManager), nameof(TutorialManager.Awake))]
    [HarmonyPostfix]
    public static void TutorialManager_Awake_Postfix()
    {
        if (!IsStopNGoEnabled) return;
        AmongUsClient.Instance.StartCoroutine(FreeplayStartDelay().WrapToIl2Cpp());
    }

    private static System.Collections.IEnumerator FreeplayStartDelay()
    {
        yield return new UnityEngine.WaitForSeconds(1f);
        StopNGoMode.Activate();
    }
}