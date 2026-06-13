using TORWL.Options.Roles.Coven;
using TORWL.Features;
using Il2CppSystem.Text;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using UnityEngine;

namespace TORWL.Roles.Coven;

public class AlchemistRole(System.IntPtr ptr) : RoleBehaviour(ptr), ICovenRole
{
    public string RoleName => "Alchemist";
    public string RoleDescription => "Brew random potions each round.";
    public string RoleLongDescription => "Each round, brew a random potion — effects include\nroleblocking, speed boosts, or a one-shot kill.";
    public Color RoleColor => LaunchpadPalette.AlchemistColor;
    public TORWLFactions Faction => TORWLFactions.Alchemica;
    public override bool IsDead => false;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        CanGetKilled = true,
        Icon = LaunchpadAssets.Alchemist,
        FreeplayFolder = "Coven",
    };

    public override void AppendTaskHint(StringBuilder taskStringBuilder) { }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        playerControl.SpawnCovenTaskHeader();
    }
}