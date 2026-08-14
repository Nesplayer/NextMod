using NEXT.Features;
using MiraAPI.Roles;
using System;
using UnityEngine;

namespace NEXT.Roles.Crewmate;

public class MedicRole(IntPtr ptr) : CrewmateRole(ptr), ICrewmateRole
{
    public string RoleName => "Medic";
    public string RoleDescription => "Help the crewmates by reviving dead players.";
    public string RoleLongDescription => "Use your revive ability to bring dead bodies\nback to life!";
    public Color RoleColor => LaunchpadPalette.MedicColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public NEXTFactions Faction => NEXTFactions.CrewPower;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.Medic,
    };
    public override void SpawnTaskHeader(PlayerControl playerControl)
        {
            playerControl.SpawnCrewmateTaskHeader();
        }
}
