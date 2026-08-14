using NEXT.Features;
using MiraAPI.Roles;
using NEXT.Components;
using System;
using UnityEngine;

namespace NEXT.Roles.Impostor
{
    // Corrected constructor syntax
    public class SilencerRole : ImpostorRole, IImpostorRole
    {
        public SilencerRole(IntPtr ptr) : base(ptr) { }

        public string RoleName => "Silencer";
        public string RoleDescription => "Silence players during meetings!";
        public string RoleLongDescription => "Silence players during meetings.\nDisables the ability to send messages in chat.";
        public Color RoleColor => LaunchpadPalette.SilencerColor;
        public ModdedRoleTeams Team => ModdedRoleTeams.Impostor;
        public NEXTFactions Faction => NEXTFactions.ImpPower;

        public CustomRoleConfiguration Configuration => new(this)
        {
            Icon = LaunchpadAssets.Silencer,
            UseVanillaKillButton = false,
        };

        public override void SpawnTaskHeader(PlayerControl playerControl)
        {
            playerControl.SpawnImpostorTaskHeader();
        }
    }
}
