using AmongUs.GameOptions;
using Il2CppSystem.Text;
using NEXT.Features;
using NEXT.Options.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using UnityEngine;

namespace NEXT.Roles.Neutral;

public class NeutralKillerRole(System.IntPtr ptr) : RoleBehaviour(ptr), INeutralRole
{
    public string RoleName => "Neutral Killer";

    public string RoleDescription => "Neutral who can kill.\nKill players to win the game alone.";

    public string RoleLongDescription => "Neutral who can kill.\nKill players to win the game alone. Seems quite easy right?";

    public Color RoleColor => LaunchpadPalette.NeutralKillerColor;
    public NEXTFactions Faction => NEXTFactions.NeutKilling;

    public override bool IsDead => false;

    public CustomRoleConfiguration Configuration => new(this)
    {
        TasksCountForProgress = false,
        CanUseVent = OptionGroupSingleton<NeutralKillerOptions>.Instance.CanUseVents,
        GhostRole = (RoleTypes)RoleId.Get<OutcastGhostRole>(),
        Icon = LaunchpadAssets.NeutralKiller,
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