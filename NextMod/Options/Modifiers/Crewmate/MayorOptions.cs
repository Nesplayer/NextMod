using System;
using UnityEngine;
using NEXT.Features;
using NEXT.Modifiers.Game.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using MiraAPI.GameOptions.Attributes;

namespace NEXT.Options.Modifiers.Crewmate;

public class MayorOptions : AbstractOptionGroup<MayorModifier>
{
    public override string GroupName => "Mayor";
    public override Color GroupColor => LaunchpadPalette.MayorMenu;
    public override Func<bool> GroupVisible =>
        () => OptionGroupSingleton<CrewmateModifierOptions>.Instance.MayorChance > 0;
    
    [ModdedNumberOption("Amount", 0f, 5f, 1f, suffixType: MiraNumberSuffixes.None, zeroInfinity: true)]
    public float MayorAmount { get; set; } = 1;

    [ModdedNumberOption("Extra Votes", 1, 3)]
    public float ExtraVotes { get; set; } = 1;

    [ModdedToggleOption("Allow Multiple Votes on Same Player")]
    public bool AllowVotingTwice { get; set; } = true;
}