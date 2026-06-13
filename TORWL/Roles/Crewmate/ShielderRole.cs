using MiraAPI.GameOptions;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using UnityEngine;
using TORWL.Features;
using System;

namespace TORWL.Roles.Crewmate;

/// <summary>
/// Shielder — a crewmate who can protect one other player from a single kill attempt.
/// </summary>
public class ShielderRole(IntPtr ptr) : CrewmateRole(ptr), ICrewmateRole
{
    public string RoleName => "Shielder";

    public string RoleDescription => "Protect a crewmate from a single kill attempt.";

    public string RoleLongDescription =>
        "You can shield one other crewmate at a time.\n" +
        "If someone tries to kill your shielded target, the kill is blocked and you are notified —\n" +
        "but you won't know who attacked them.\n" +
        "The shield is consumed after blocking one kill. You cannot shield yourself.\n";

    public Color RoleColor => LaunchpadPalette.ShielderColor; // soft teal-green

    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        CanUseVent = false,
        CanUseSabotage = false,
        UseVanillaKillButton = false,
        TasksCountForProgress = true,
        Icon = LaunchpadAssets.Shielder,
    };

    /// <summary>The player currently holding the Shielder's shield. Null if no shield is active.</summary>
    public PlayerControl? ShieldedPlayer { get; set; }

    /// <summary>
    /// Whether the kill-attempt notification should be shown to the Shielder
    /// this frame (set by the murder patch, cleared after display).
    /// </summary>
    public bool PendingBlockNotification { get; set; }

    public TORWLFactions Faction => TORWLFactions.CrewProtective;

}
