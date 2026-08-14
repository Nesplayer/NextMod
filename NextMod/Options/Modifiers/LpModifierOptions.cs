using MiraAPI.GameOptions;
using NEXT.Features;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using UnityEngine;

namespace NEXT.Options.Modifiers;

public class LpModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "Modifier Options";
    public override Color GroupColor => LaunchpadPalette.LpMenu;
    public override uint GroupPriority => 0;
    public override bool ShowInModifiersMenu => true;

    [ModdedNumberOption("Player Modifier Limit", 0f, 10, 1, suffixType: MiraNumberSuffixes.None, zeroInfinity: true)]
    public float ModifierLimit { get; set; } = 1f;
}