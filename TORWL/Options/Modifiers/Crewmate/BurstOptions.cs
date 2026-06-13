using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using TORWL.Features;
using TORWL.Modifiers.Game.Crewmate;
using System;
using UnityEngine;
using MiraAPI.Utilities;

namespace TORWL.Options.Modifiers.Crewmate;

    public class BurstOptions : AbstractOptionGroup<BurstModifier>
    {
        public override string GroupName => "Burst";
        public override Color GroupColor => LaunchpadPalette.BurstMenu;
        public override Func<bool> GroupVisible =>
            () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.BurstChance > 0;

        [ModdedNumberOption("Amount", 1, 10, zeroInfinity:true)]
        public float BurstAmount { get; set; } = 1f;
        
        [ModdedNumberOption("Burst Radius", 30, 60, 5, MiraNumberSuffixes.Multiplier)]
        public float BurstRadius { get; set; } = 45f;
    }
