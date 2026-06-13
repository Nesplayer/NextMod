using TORWL.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace TORWL.Options.Roles.Coven;

public class HexbinderOptions : AbstractOptionGroup<HexbinderRole>
{
    public override string GroupName => "Hexbinder";
    
    [ModdedNumberOption("Max Amounts of Hex Uses", 0, 8, 1, zeroInfinity:true)]
    public float Uses { get; set; } = 4f;

    [ModdedNumberOption("Hex Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float HexCooldown { get; set; } = 20f;

    [ModdedNumberOption("Meetings Until Death", 1, 5, 1)]
    public float MeetingsUntilDeath { get; set; } = 2f;
}