using System;
using NEXT.Features;
using NEXT.Modifiers.Game.Universal;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using UnityEngine;
using MiraAPI.Utilities;

namespace NEXT.Options.Modifiers.Universal
{
    public class FlashOptions : AbstractOptionGroup<FlashModifier>
    {
        public override string GroupName => "Flash";
        public override Color GroupColor => LaunchpadPalette.FlashMenu;
        public override Func<bool> GroupVisible =>
            () => OptionGroupSingleton<UniversalModifierOptions>.Instance.FlashChance > 0;

        [ModdedNumberOption("Amount", 0f, 5f, 1f, suffixType: MiraNumberSuffixes.None, zeroInfinity: true)]
        public float FlashAmount { get; set; } = 1f;
    }
}