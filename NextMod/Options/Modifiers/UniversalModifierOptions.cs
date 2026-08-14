using MiraAPI.GameOptions;
using NEXT.Features;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using UnityEngine;

namespace NEXT.Options.Modifiers;

public class UniversalModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "Universal Modifiers";
    public override Color GroupColor => LaunchpadPalette.UniversalMenu;
    public override bool ShowInModifiersMenu => true;
    public override uint GroupPriority => 1;
    
    [ModdedNumberOption("Flash Chance", 0f, 100f, 10f, suffixType: MiraNumberSuffixes.Percent)]
    public float FlashChance { get; set; } = 0f;

    [ModdedNumberOption("V.I.P Chance", 0f, 100f, 10f, suffixType: MiraNumberSuffixes.Percent)]
    public float KingChance { get; set; } = 0f;
}