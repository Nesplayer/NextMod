using TORWL.Options.Roles.Coven;
using TORWL.Features;
using Il2CppSystem.Text;
using MiraAPI.GameOptions;
using MiraAPI.Roles;
using UnityEngine;

namespace TORWL.Roles.Coven;

public class ScribeRole(System.IntPtr ptr) : RoleBehaviour(ptr), ICovenRole, IWikiRole
{

    public string RoleName => "Scribe";
    public string RoleDescription => "Gather info to betray players.";
    public string RoleLongDescription => "Gather the player role alignment to use it to your\nwill, which can be any way.";
    public Color RoleColor => LaunchpadPalette.ScribeColor;
    public TORWLFactions Faction => TORWLFactions.Hexcraft;
    public string WikiDescription => "test";
    public override bool IsDead => false;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        CanGetKilled = true,
        Icon = LaunchpadAssets.Scribe,
        FreeplayFolder = "Coven",
    };

    public override void AppendTaskHint(StringBuilder taskStringBuilder)
    {
        // remove default task hint
    }
    
    public override void SpawnTaskHeader(PlayerControl playerControl)
    {
        playerControl.SpawnCovenTaskHeader();
    }
}