using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using NEXT.Roles.Neutral;

namespace NEXT.Options.Roles.Neutral;

public sealed class SurvivorOptions : AbstractOptionGroup<SurvivorRole>
{
    public override string GroupName => "Survivor";

    [ModdedNumberOption("Vest Cooldown", 10, 60, 2.5f, MiraNumberSuffixes.Seconds)]
    public float VestCooldown { get; set; } = 25;

    [ModdedNumberOption("Vest Duration", 5, 15, 1f, MiraNumberSuffixes.Seconds)]
    public float VestDuration { get; set; } = 10f;

    [ModdedNumberOption("Max Number Of Vests", 1f, 15f, 1f, MiraNumberSuffixes.None, "0")]
    public float MaxVests { get; set; } = 10f;
}