using TORWL.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace TORWL.Options.Roles.Coven;

public class AlchemistOptions : AbstractOptionGroup<AlchemistRole>
{
    public override string GroupName => "Alchemist";

    [ModdedNumberOption("Brew Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float BrewCooldown { get; set; } = 25f;

    [ModdedNumberOption("Potion Effect Duration", 5, 60, 5, MiraNumberSuffixes.Seconds)]
    public float EffectDuration { get; set; } = 10f;

    [ModdedToggleOption("Can Brew Kill Potion")]
    public bool CanBrewKill { get; set; } = false;
}