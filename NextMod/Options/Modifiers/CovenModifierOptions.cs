using MiraAPI.GameOptions;
using NEXT.Features;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using UnityEngine;

namespace NEXT.Options.Modifiers;

public class CovenModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "Coven Modifiers";
    public override bool ShowInModifiersMenu => true;
    public override Color GroupColor => LaunchpadPalette.CovenMenu;

    [ModdedNumberOption("Rune-Bound Chance", 0f, 100f, 10f, suffixType: MiraNumberSuffixes.Percent)]
    public float RuneBoundChance { get; set; } = 0f;
}