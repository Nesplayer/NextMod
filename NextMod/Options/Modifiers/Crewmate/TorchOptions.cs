using System;
using NEXT.Features;
using NEXT.Modifiers.Game.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;
using UnityEngine;

namespace NEXT.Options.Modifiers.Crewmate;

public class TorchOptions : AbstractOptionGroup<TorchModifier>
{
    public override string GroupName => "Torch";
    public override Color GroupColor => LaunchpadPalette.TorchMenu;
    public override Func<bool> GroupVisible =>
        () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.TorchChance > 0;
    
    [ModdedNumberOption("Amount", 0f, 5f, 1f, suffixType: MiraNumberSuffixes.None, zeroInfinity: true)]
    public float TorchAmount { get; set; } = 1;

    [ModdedToggleOption("Use Hide N Seek Flashlight")]
    public bool UseFlashlight { get; set; } = true;

    public ModdedNumberOption TorchFlashlightSize { get; } = new("Flashlight Size", .25f, 0.1f, .5f, 0.05f, MiraNumberSuffixes.Multiplier)
    {
        Visible = () => OptionGroupSingleton<TorchOptions>.Instance.UseFlashlight,
    };
}