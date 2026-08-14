using NEXT.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace NEXT.Options.Roles.Coven;

public class ShroudweaverOptions : AbstractOptionGroup<ShroudweaverRole>
{
    public override string GroupName => "Shroudweaver";
    
    [ModdedNumberOption("Max Shroud Uses", 1, 8, zeroInfinity:true)]
    public float MaxUses { get; set; } = 4;

    [ModdedNumberOption("Shroud Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float ShroudCooldown { get; set; } = 20f;
}