using TORWL.Options.Roles.Coven;
using TORWL.Features;
using Il2CppSystem.Text;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using UnityEngine;

namespace TORWL.Roles.Coven;

public class DominatorRole(System.IntPtr ptr) : RoleBehaviour(ptr), ICovenRole
{
    public string RoleName => "Dominator";
    public string RoleDescription => "Hijack a crewmate's ability once.";
    public string RoleLongDescription => "Once per game, take control of a crewmate's ability\nand force them to use it on your command.";
    public Color RoleColor => LaunchpadPalette.DominatorColor;
    public TORWLFactions Faction => TORWLFactions.Dominion;
    public override bool IsDead => false;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        CanGetKilled = true,
        Icon = LaunchpadAssets.Dominator,
        FreeplayFolder = "Coven",
    };

    public override void AppendTaskHint(StringBuilder taskStringBuilder) { }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        playerControl.SpawnCovenTaskHeader();
    }
}