using NEXT.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using MiraAPI.Utilities;

namespace NEXT.Options.Roles.Coven;

public class TavernKeeperOptions : AbstractOptionGroup<TavernKeeperRole>
{
    public override string GroupName => "Tavern Keeper";
    [ModdedNumberOption("Role Block Cooldown", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float RoleBlockCooldown { get; set; } = 20;
    [ModdedNumberOption("Max Role Block Uses", 0, 10, zeroInfinity:true)]
    public float RoleBlockUses { get; set; } = 5;
    [ModdedNumberOption("Role Block Duration", 0, 60, 5, MiraNumberSuffixes.Seconds)]
    public float RoleBlockDuration { get; set; } = 20;
}