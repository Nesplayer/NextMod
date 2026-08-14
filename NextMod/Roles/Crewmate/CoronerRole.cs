using NEXT.Features;
using MiraAPI.Roles;
using System;
using UnityEngine;

namespace NEXT.Roles.Crewmate;

public class CoronerRole(IntPtr ptr) : CrewmateRole(ptr), ICrewmateRole
{
    public string RoleName => "Coroner";
    public string RoleDescription => "Freeze bodies to prevent them from disappearing.";
    public string RoleLongDescription => RoleDescription;
    public Color RoleColor => LaunchpadPalette.CoronerColor;
    public ModdedRoleTeams Team => ModdedRoleTeams.Crewmate;
    public NEXTFactions Faction => NEXTFactions.CrewSupport;
    public CustomRoleConfiguration Configuration => new(this)
    {
        Icon = LaunchpadAssets.Coroner,
    };
    public override void SpawnTaskHeader(PlayerControl playerControl)
        {
            playerControl.SpawnCrewmateTaskHeader();
        }
}