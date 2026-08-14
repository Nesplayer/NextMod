using NEXT.Roles.Impostor;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace NEXT.Options.Roles.Impostor;

public class SilencerOptions : AbstractOptionGroup<SilencerRole>
{
    public override string GroupName => "Silencer";

    [ModdedNumberOption("Silence Cooldown", 0, 120, 5, MiraNumberSuffixes.Seconds)]
    public float SilenceCooldown { get; set; } = 35;

    [ModdedNumberOption("Max Silence Uses", 0, 12, 2, zeroInfinity: true)]
    public float SilenceUses { get; set; } = 0;
}