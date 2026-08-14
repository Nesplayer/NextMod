using NEXT.Roles.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace NEXT.Options.Roles.Crewmate;

public class ShielderOptions : AbstractOptionGroup<ShielderRole>
{
    public override string GroupName => "Shielder";

    [ModdedNumberOption("Shield Cooldown", 5, 60, 5, MiraNumberSuffixes.Seconds)]
    public float ShieldCooldown { get; set; } = 25;

    [ModdedNumberOption("Max Shields", 0, 8, 1, zeroInfinity: true)]
    public float MaxShields { get; set; } = 3;
}