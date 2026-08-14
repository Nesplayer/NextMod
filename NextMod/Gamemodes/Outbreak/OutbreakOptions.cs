using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using System;
using NEXT.Options;
using UnityEngine;

namespace NEXT.Features.Outbreak;

public class OutbreakOptions : AbstractOptionGroup
{
    public override string GroupName => "Outbreak";
    public override Color GroupColor => new Color32(50, 205, 50, 255);

    public override Func<bool> GroupVisible => () =>
        OptionGroupSingleton<GeneralOptions>.Instance.Gamemode.Value == (int)LaunchpadGamemode.Outbreak;

    // These are exposed as lobby options but the hardcoded constants in OutbreakMode
    // take precedence for the radius and base infection time.
    // Add more tunables here if you want them host-adjustable in future.

    [ModdedNumberOption("Task Delay Per Task", 0f, 10f, 0.5f, MiraNumberSuffixes.Seconds)]
    public float TaskDelayPerTask { get; set; } = 2f;
}
