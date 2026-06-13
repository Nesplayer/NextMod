using TORWL.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace TORWL.Options.Roles.Coven;

public class PoisonerOptions : AbstractOptionGroup<PoisonerRole>
{
    public override string GroupName => "Poisoner";
    
    [ModdedNumberOption("Max Amounts of Poison Uses", 0, 8, 1, zeroInfinity:true)]
    public float Uses { get; set; } = 4f;

    [ModdedNumberOption("Poison Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float PoisonCooldown { get; set; } = 20f;

    [ModdedToggleOption("Can Cure")]
    public bool CanCure { get; set; } = true;
}