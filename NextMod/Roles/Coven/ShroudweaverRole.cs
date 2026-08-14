using NEXT.Options.Roles.Coven;
using NEXT.Features;
using Il2CppSystem.Text;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using UnityEngine;

namespace NEXT.Roles.Coven;

public class ShroudweaverRole(System.IntPtr ptr) : RoleBehaviour(ptr), ICovenRole
{
    public string RoleName => "Shroudweaver";
    public string RoleDescription => "Make players appear dead to scanners.";
    public string RoleLongDescription => "Shroud a player each round, making them appear dead\non Scientist vitals and the Coroner until the next meeting.";
    public Color RoleColor => LaunchpadPalette.ShroudweaverColor;
    public NEXTFactions Faction => NEXTFactions.Hexcraft;
    public override bool IsDead => false;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        CanGetKilled = true,
        Icon = LaunchpadAssets.Shroudweaver,
        FreeplayFolder = "Coven",
    };

    public override void AppendTaskHint(StringBuilder taskStringBuilder) { }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        playerControl.SpawnCovenTaskHeader();
    }
}