using NEXT.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace NEXT.Options.Roles.Coven;

public class DominatorOptions : AbstractOptionGroup<DominatorRole>
{
    public override string GroupName => "Dominator";
    
    [ModdedNumberOption("Max Dominate Uses", 1, 8, zeroInfinity:true)]
    public float MaxUses { get; set; } = 4;

    [ModdedNumberOption("Dominate Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float DominateCooldown { get; set; } = 30f;
}