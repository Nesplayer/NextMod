using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;
using NEXT.Options;
using UnityEngine;

namespace NEXT.Features.StopNGo;

public class StopNGoOptions : AbstractOptionGroup
{
    public override string GroupName => "Stop 'n Go";
    public override Color GroupColor => new Color32(255, 80, 80, 255);

    public override Func<bool> GroupVisible => () =>
        OptionGroupSingleton<GeneralOptions>.Instance.Gamemode.Value == (int)LaunchpadGamemode.StopNGo;

    [ModdedNumberOption("Green Light Duration", 3f, 30f, 1f, MiraNumberSuffixes.Seconds)]
    public float GreenDuration { get; set; } = 8f;

    [ModdedNumberOption("Red Light Duration", 2f, 20f, 1f, MiraNumberSuffixes.Seconds)]
    public float RedDuration { get; set; } = 5f;
}