using TORWL.Features;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using UnityEngine;
using MiraAPI.Utilities;
using System;
using TORWL.Modifiers.Game.Coven;

namespace TORWL.Options.Modifiers.Coven;

public class RuneBoundOptions : AbstractOptionGroup<RuneBoundModifier>
{
    public override string GroupName => "Rune-Bound";
    public override Color GroupColor => LaunchpadPalette.RuneBoundMenu;
    public override Func<bool> GroupVisible =>
        () => OptionGroupSingleton<CovenModifierOptions>.Instance.RuneBoundChance > 0;

    [ModdedNumberOption("Amount", 0f, 5f, 1f, suffixType: MiraNumberSuffixes.None, zeroInfinity: true)]
    public float RBAmount { get; set; } = 1;
    
    [ModdedNumberOption("Cooldown Reduction (%)", 5f, 50f, 5f, MiraNumberSuffixes.Percent)]
    public float CooldownReduction { get; set; } = 15f;
}