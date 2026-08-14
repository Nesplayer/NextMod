using NEXT.Options.Roles.Coven;
using NEXT.Features;
using Il2CppSystem.Text;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using UnityEngine;

namespace NEXT.Roles.Coven;

public class HexbinderRole(System.IntPtr ptr) : RoleBehaviour(ptr), ICovenRole
{
    public string RoleName => "Hexbinder";
    public string RoleDescription => "Curse players with a delayed death.";
    public string RoleLongDescription => "Place a hex on a player that will kill them\nafter a configurable number of meetings pass.";
    public Color RoleColor => LaunchpadPalette.HexbinderColor;
    public NEXTFactions Faction => NEXTFactions.Hexcraft;
    public override bool IsDead => false;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        CanGetKilled = true,
        Icon = LaunchpadAssets.Hexbinder,
        FreeplayFolder = "Coven",
    };

    public override void AppendTaskHint(StringBuilder taskStringBuilder) { }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        playerControl.SpawnCovenTaskHeader();
    }
}