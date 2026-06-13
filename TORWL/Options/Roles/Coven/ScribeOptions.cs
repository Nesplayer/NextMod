using TORWL.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace TORWL.Options.Roles.Coven;

public class ScribeOptions : AbstractOptionGroup<ScribeRole>
{
    public override string GroupName => "Scribe";
    [ModdedNumberOption("Insight Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float InsightCooldown { get; set; } = 20;
    [ModdedNumberOption("Max Insight Uses", 0, 10, zeroInfinity:true)]
    public float MaxInsightUses { get; set; } = 5;
}