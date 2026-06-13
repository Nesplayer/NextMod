using TORWL.Roles.Coven;
using TORWL.Features;
using TORWL.Options.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using MiraAPI.Utilities;
using TORWL.Managers;
using UnityEngine;

namespace TORWL.Buttons.Coven;

public class BrewButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Brew";
    public override Color TextOutlineColor => LaunchpadPalette.AlchemistColor;
    public override float Cooldown => OptionGroupSingleton<AlchemistOptions>.Instance.BrewCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.Brew;
    public override bool TimerAffectedByPlayer => true;

    public override bool Enabled(RoleBehaviour? role) => role is AlchemistRole;

    public override PlayerControl? GetTarget() =>
        PlayerControl.LocalPlayer.GetClosestPlayer(true, Distance, false);

    public override void SetOutline(bool active) =>
        Target?.cosmetics.SetOutline(active, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.AlchemistColor));

    public override bool IsTargetValid(PlayerControl? target) => true;

    protected override void OnClick()
    {
        if (Target == null) return;
        AlchemistManager.ThrowPotion(Target);
    }
}