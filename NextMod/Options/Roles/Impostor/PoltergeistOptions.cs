using NEXT.Roles.Impostor;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace NEXT.Options.Roles.Impostor;

public class PoltergeistOptions : AbstractOptionGroup<PoltergeistRole>
{
    public override string GroupName => "Poltergeist";

    [ModdedNumberOption("Curse Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float CurseCooldown { get; set; } = 10f;

    [ModdedNumberOption("Curse Durations", 5, 60, 5, MiraNumberSuffixes.Seconds)]
    public float CurseDuration { get; set; } = 10;
}