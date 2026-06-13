using TORWL.Features;
using TORWL.Modifiers.Game.Universal;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using UnityEngine;
using MiraAPI.Utilities;
using System;

namespace TORWL.Options.Modifiers.Universal
{
    public class KingOptions : AbstractOptionGroup<KingModifier>
    {
        public override string GroupName => "V.I.P";
        public override Color GroupColor => LaunchpadPalette.KingMenu;
        public override Func<bool> GroupVisible =>
            () => OptionGroupSingleton<UniversalModifierOptions>.Instance.KingChance > 0;

        [ModdedNumberOption("Amount", 0f, 5f, 1f, suffixType: MiraNumberSuffixes.None, zeroInfinity: true)]
        public float KingAmount { get; set; } = 1f;
    }
}