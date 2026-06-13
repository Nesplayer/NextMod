using TORWL.Options.Roles.Coven;
using TORWL.Features;
using Il2CppSystem.Text;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using UnityEngine;

namespace TORWL.Roles.Coven;

public class PoisonerRole(System.IntPtr ptr) : RoleBehaviour(ptr), ICovenRole
{
    public string RoleName => "Poisoner";
    public string RoleDescription => "Poison players to kill at the next meeting.";
    public string RoleLongDescription => "Poison a player each round. They will die at the\nend of the next meeting unless you cure them.";
    public Color RoleColor => LaunchpadPalette.PoisonerColor;
    public TORWLFactions Faction => TORWLFactions.Alchemica;
    public override bool IsDead => false;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        CanGetKilled = true,
        Icon = LaunchpadAssets.Toxifier,
        FreeplayFolder = "Coven",
    };

    public override void AppendTaskHint(StringBuilder taskStringBuilder) { }

    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        playerControl.SpawnCovenTaskHeader();
    }
}