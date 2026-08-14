using NEXT.Roles.Neutral;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;

namespace NEXT.Options.Roles.Neutral;

public class NeutralKillerOptions : AbstractOptionGroup<NeutralKillerRole>
{
    public override string GroupName => "Neutral Killer";
    [ModdedNumberOption("Kill Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float NeutralKillCooldown { get; set; } = 20;
    [ModdedToggleOption("Can Use Vents")]
    public bool CanUseVents { get; set; } = true;
}