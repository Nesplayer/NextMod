using Reactor.Networking.Attributes;
using TORWL.Roles.Crewmate;
using TORWL.Networking;

namespace TORWL.Patches.Roles.Shielder;

/// <summary>
/// RPCs that keep the shielded-player state in sync across all clients.
///
/// Because TORWLaunchpad is a client-side mod, we use Reactor's [MethodRpc] to
/// broadcast state changes so every player's <see cref="ShielderRole.ShieldedPlayer"/>
/// stays consistent — crucial for the murder patch to fire correctly on the
/// killer's machine.
/// </summary>
public static class ShielderRpc
{
    /// <summary>
    /// Called by the Shielder's client when they apply a shield.
    /// Broadcasts to all clients so they update the Shielder's state.
    /// </summary>
    [MethodRpc((uint)LaunchpadRpc.SetShield)]
    public static void RpcSetShield(PlayerControl shielderPlayer, PlayerControl targetPlayer)
    {
        if (shielderPlayer.Data.Role is ShielderRole shielder)
        {
            shielder.ShieldedPlayer = targetPlayer;
        }
    }

    /// <summary>
    /// Called after a shield is consumed (kill blocked) so all clients clear the shield.
    /// </summary>
    [MethodRpc((uint)LaunchpadRpc.ClearShield)]
    public static void RpcClearShield(PlayerControl shielderPlayer)
    {
        if (shielderPlayer.Data.Role is ShielderRole shielder)
        {
            shielder.ShieldedPlayer = null;
        }
    }
}
