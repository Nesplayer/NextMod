using NEXT.Roles.Crewmate;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace NEXT.Options.Roles.Crewmate;

public class TeleporterOptions : AbstractOptionGroup<TeleporterRole>
{
    public override string GroupName => "Teleporter";

    public ModdedNumberOption TeleportCooldown { get; set; } = new("Teleport Cooldown", 10, 5, 60, 2.5f, MiraNumberSuffixes.Seconds);

    [ModdedNumberOption("Teleport Duration", 5, 25, 1, MiraNumberSuffixes.Seconds)]
    public float TeleportDuration { get; set; } = 10;

    [ModdedNumberOption("Zoom Distance", 4, 15)]
    public float ZoomDistance { get; set; } = 6;
    
    [ModdedNumberOption("Max Teleport Uses", 0, 10, zeroInfinity: true)]
    public float TeleportUses { get; set; } = 3;
}