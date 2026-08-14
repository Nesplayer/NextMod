using System.Collections;
using System.Collections.Generic;
using AmongUs.GameOptions;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using Reactor.Networking.Attributes;
using NEXT.Networking;
using UnityEngine;

namespace NEXT.Features.Outbreak;

/// <summary>
/// Outbreak — one player starts as Patient 0 (infected).
/// Infected players spread infection to nearby crewmates over time.
/// Crewmates completing tasks slows the infection rate globally.
/// Infected win if everyone is infected; crew wins if all tasks are done.
/// </summary>
public static class OutbreakMode
{
    // ── State ─────────────────────────────────────────────────────────────────

    public static bool IsActive { get; private set; }

    public static int Patient0Id { get; set; } = -1;

    /// <summary>Tracks infection progress per player (0..1). Host only.</summary>
    private static readonly Dictionary<byte, float> _infectionProgress = new();

    /// <summary>Delay (in seconds) added per completed task.</summary>
    public const float TaskDelayPerTask = 2f;

    /// <summary>Accumulated extra seconds added to infection time via tasks.</summary>
    public static float TaskBonus { get; private set; } = 0f;

    /// <summary>Hardcoded infection radius (world units).</summary>
    public const float InfectionRadius = 2.5f;

    /// <summary>Base seconds to fully infect a player standing inside the radius.</summary>
    public const float BaseInfectionTime = 5f;

    private static Coroutine? _infectionLoop;

    private static OutbreakOptions Options =>
        OptionGroupSingleton<OutbreakOptions>.Instance;

    // ── Activation ────────────────────────────────────────────────────────────

    public static void Activate()
    {
        if (!AmongUsClient.Instance.AmHost) return;
        IsActive = true;
        TaskBonus = 0f;
        _infectionProgress.Clear();
        PlayerControl.LocalPlayer.RpcSetOutbreakActive(true);
        _infectionLoop = AmongUsClient.Instance.StartCoroutine(InfectionLoop().WrapToIl2Cpp());
        Patient0Id = -1;
        PatientZeroModifier.IsAssigned = false;
    }

    public static void Deactivate()
    {
        IsActive = false;
        _infectionProgress.Clear();
        if (_infectionLoop != null)
        {
            AmongUsClient.Instance.StopCoroutine(_infectionLoop);
            _infectionLoop = null;
        }
        if (AmongUsClient.Instance.AmHost)
            PlayerControl.LocalPlayer.RpcSetOutbreakActive(false);
        Patient0Id = -1;
        PatientZeroModifier.IsAssigned = false;
    }

    // ── Task bonus (host only) ────────────────────────────────────────────────

    /// <summary>Called by the patch whenever any crewmate completes a task.</summary>
    public static void OnTaskCompleted()
    {
        if (!IsActive || !AmongUsClient.Instance.AmHost) return;
        TaskBonus += TaskDelayPerTask;
    }

    /// <summary>Current effective infection time including task slowdown.</summary>
    public static float EffectiveInfectionTime => BaseInfectionTime + TaskBonus;

    // ── Infection loop (host only, runs every FixedUpdate tick via coroutine) ──

    private static IEnumerator InfectionLoop()
    {
        while (IsActive)
        {
            yield return new WaitForFixedUpdate();
            TickInfection();
        }
    }

    private static void TickInfection()
    {
        foreach (var infected in PlayerControl.AllPlayerControls)
        {
            if (infected == null || infected.Data == null || infected.Data.IsDead) continue;
            if (!infected.HasModifier<InfectedModifier>() && !infected.HasModifier<PatientZeroModifier>()) continue;

            foreach (var target in PlayerControl.AllPlayerControls)
            {
                if (target == null || target.Data == null || target.Data.IsDead) continue;
                if (target.HasModifier<InfectedModifier>() || target.HasModifier<PatientZeroModifier>()) continue;

                var dist = Vector2.Distance(
                    (Vector2)infected.transform.position,
                    (Vector2)target.transform.position);

                if (dist > InfectionRadius)
                {
                    // Decay progress slowly when out of range (but don't go below 0)
                    if (_infectionProgress.TryGetValue(target.PlayerId, out var current) && current > 0f)
                    {
                        _infectionProgress[target.PlayerId] = Mathf.Max(0f, current - Time.fixedDeltaTime * 0.1f);
                        PlayerControl.LocalPlayer.RpcSetOutbreakProgress(target.PlayerId, _infectionProgress[target.PlayerId]);
                    }
                    continue;
                }

                // Accumulate infection
                if (!_infectionProgress.ContainsKey(target.PlayerId))
                    _infectionProgress[target.PlayerId] = 0f;

                _infectionProgress[target.PlayerId] += Time.fixedDeltaTime / EffectiveInfectionTime;
                _infectionProgress[target.PlayerId] = Mathf.Clamp01(_infectionProgress[target.PlayerId]);

                PlayerControl.LocalPlayer.RpcSetOutbreakProgress(target.PlayerId, _infectionProgress[target.PlayerId]);

                if (_infectionProgress[target.PlayerId] >= 1f)
                {
                    _infectionProgress.Remove(target.PlayerId);
                    PlayerControl.LocalPlayer.RpcInfectPlayer(target.PlayerId);
                }
            }
        }
    }

    // ── Win condition (host only) ─────────────────────────────────────────────

    public static void CheckWinCondition()
    {
        if (!IsActive || !AmongUsClient.Instance.AmHost) return;

        bool anyClean = false;
        foreach (var pc in PlayerControl.AllPlayerControls)
        {
            if (pc == null || pc.Data == null || pc.Data.IsDead) continue;
            if (!pc.HasModifier<InfectedModifier>() && !pc.HasModifier<PatientZeroModifier>())
            {
                anyClean = true;
                break;
            }
        }

        if (!anyClean)
        {
            Deactivate();
            GameManager.Instance?.RpcEndGame((GameOverReason)OutbreakGameOverReason.InfectedWin, false);
        }
    }

    public static void CheckCrewWin()
    {
        if (!IsActive || !AmongUsClient.Instance.AmHost) return;

        if (GameData.Instance == null) return;
        if (GameData.Instance.TotalTasks > 0 &&
            GameData.Instance.CompletedTasks >= GameData.Instance.TotalTasks)
        {
            Deactivate();
            GameManager.Instance?.RpcEndGame((GameOverReason)OutbreakGameOverReason.CrewWin, false);
        }
    }

    // ── RPCs ──────────────────────────────────────────────────────────────────

    [MethodRpc((uint)LaunchpadRpc.OutbreakSetActive)]
    public static void RpcSetOutbreakActive(this PlayerControl sender, bool active)
    {
        IsActive = active;
        OutbreakHud.SetVisible(active);
        if (!active) OutbreakHud.ClearAllProgress();
    }

    [MethodRpc((uint)LaunchpadRpc.OutbreakSetProgress)]
    public static void RpcSetOutbreakProgress(this PlayerControl sender, byte targetId, float progress)
    {
        OutbreakHud.SetInfectionProgress(targetId, progress);
    }

    [MethodRpc((uint)LaunchpadRpc.OutbreakInfectPlayer)]
    public static void RpcInfectPlayer(this PlayerControl sender, byte targetId)
    {
        // Prefer GameData lookup but fall back to AllPlayerControls scan —
        // in freeplay, GameData PlayerInfo.Object is null for dummy bots
        // beyond the first couple, which silently breaks infection chains.
        PlayerControl? target = null;

        var info = GameData.Instance?.GetPlayerById(targetId);
        if (info != null) target = info.Object;

        if (target == null)
        {
            foreach (var pc in PlayerControl.AllPlayerControls)
            {
                if (pc != null && pc.PlayerId == targetId) { target = pc; break; }
            }
        }

        if (target == null) return;

        OutbreakHud.SetInfectionProgress(targetId, 0f);

        var modComp = target.GetComponent<ModifierComponent>();
        modComp?.AddModifier<InfectedModifier>();

        if (AmongUsClient.Instance.AmHost)
            CheckWinCondition();
    }
}

public enum OutbreakGameOverReason
{
    InfectedWin = 11,
    CrewWin = 12,
}