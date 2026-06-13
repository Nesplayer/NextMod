using HarmonyLib;
using MiraAPI.Utilities;
using TORWL.Roles.Crewmate;
using UnityEngine;

namespace TORWL.Patches.Roles.Shielder;

/// <summary>
/// Intercepts kill attempts. If the target is shielded by a Shielder:
///   1. The kill is blocked (no body, no death).
///   2. The Shielder is notified with a HUD flash/text.
///   3. The shield is consumed.
///
/// This patch runs client-side on the KILLER's machine (where CheckMurder originates).
/// Because TORWLaunchpad is a client-side mod (all players must install), this is fine.
/// </summary>
[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CheckMurder))]
public static class ShielderMurderPatch
{
    // Return false to cancel the kill; return true to let it proceed normally.
    public static bool Prefix(PlayerControl __instance, PlayerControl target)
    {
        // Find if any alive Shielder has a shield on this target.
        ShielderRole? shielder = FindShielderFor(target);
        if (shielder == null) return true; // no shield — proceed with kill normally

        // --- Block the kill ---

        // 1. Remove the shield.
        shielder.ShieldedPlayer = null;
        ShielderRpc.RpcClearShield(shielder.Player);

        // 2. Notify the Shielder (only on the Shielder's own client).
        if (shielder.Player == PlayerControl.LocalPlayer)
        {
            NotifyShielder();
        }

        // 3. Cancel the kill.
        return false;
    }

    // -------------------------------------------------------------------------

    private static ShielderRole? FindShielderFor(PlayerControl target)
    {
        foreach (var player in PlayerControl.AllPlayerControls)
        {
            if (player.Data.IsDead) continue;
            if (player.Data.Role is not ShielderRole shielder) continue;
            if (shielder.ShieldedPlayer == target) return shielder;
        }
        return null;
    }

    private static void NotifyShielder()
    {
        // Use MiraAPI's built-in HUD notification helper if available,
        // otherwise fall back to the vanilla popup system.
        if (HudManager.Instance != null)
        {
            HudManager.Instance.Notifier.AddDisconnectMessage(
                $"<color=#52B788>Your shield blocked a kill attempt!</color>"
            );
        }
    }
}
