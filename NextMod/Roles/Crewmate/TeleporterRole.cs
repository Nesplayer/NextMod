using NEXT.Buttons.Crewmate;
using NEXT.Features;
using MiraAPI.Roles;
using System;
using UnityEngine;

namespace NEXT.Roles.Crewmate;

public class TeleporterRole(IntPtr ptr) : CrewmateRole(ptr), ICrewmateRole
{
    public string RoleName => "Teleporter";
    public string RoleLongDescription => "Zoom out and teleport across the map!";
    public string RoleDescription => RoleLongDescription;
    public Color RoleColor => LaunchpadPalette.TeleporterColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public NEXTFactions Faction => NEXTFactions.CrewPower;

    public CustomRoleConfiguration Configuration => new CustomRoleConfiguration(this)
    {
        Icon = LaunchpadAssets.Teleporter,
    };
    public override void SpawnTaskHeader(PlayerControl playerControl)
        {
            playerControl.SpawnCrewmateTaskHeader();
        }
}