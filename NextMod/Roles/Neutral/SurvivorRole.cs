using AmongUs.GameOptions;
using Il2CppSystem.Text;
using NEXT.Features;
using NEXT.Options.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using UnityEngine;

namespace NEXT.Roles.Neutral;

public class SurvivorRole(System.IntPtr ptr) : RoleBehaviour(ptr), INeutralRole
{
    public string RoleName => "Survivor";

    public string RoleDescription => "Survive till the end.";

    public string RoleLongDescription => RoleDescription;

    public Color RoleColor => LaunchpadPalette.SurvivorColor;
    public NEXTFactions Faction => NEXTFactions.Benign;

    public override bool IsDead => false;

    public CustomRoleConfiguration Configuration => new(this)
    {
        TasksCountForProgress = false,
        CanUseVent = false,
        GhostRole = (RoleTypes)RoleId.Get<OutcastGhostRole>(),
        Icon = LaunchpadAssets.Survivor,
    };

    public override void AppendTaskHint(StringBuilder taskStringBuilder)
    {
        // No task hints
    }

    public override void SpawnTaskHeader(PlayerControl playerControl)
        {
            playerControl.SpawnNeutralTaskHeader();
        }
}