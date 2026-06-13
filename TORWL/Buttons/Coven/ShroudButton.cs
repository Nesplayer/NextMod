using TORWL.Roles.Coven;
using TORWL.Features;
using TORWL.Options.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using MiraAPI.Utilities;
using TORWL.Managers;
using UnityEngine;

namespace TORWL.Buttons.Coven;

public class ShroudButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Shroud";
    public override Color TextOutlineColor => LaunchpadPalette.ShroudweaverColor;
    public override float Cooldown => OptionGroupSingleton<ShroudweaverOptions>.Instance.ShroudCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)OptionGroupSingleton<ShroudweaverOptions>.Instance.MaxUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.Shroud;
    public override bool TimerAffectedByPlayer => true;

    public override bool Enabled(RoleBehaviour? role) => role is ShroudweaverRole;
    
    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);

        Button!.usesRemainingSprite.sprite = LaunchpadAssets.Player.LoadAsset();
        Button!.usesRemainingSprite.color = LaunchpadPalette.ShroudweaverColor;
    }

    public override PlayerControl? GetTarget() =>
        PlayerControl.LocalPlayer.GetClosestPlayer(true, Distance, false);

    public override void SetOutline(bool active) =>
        Target?.cosmetics.SetOutline(active, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.ShroudweaverColor));

    public override bool IsTargetValid(PlayerControl? target) =>
        target != null && !ShroudweaverManager.IsShrouded(target.PlayerId);

    protected override void OnClick()
    {
        if (Target == null) return;
        ShroudweaverManager.ShroudPlayer(Target);
    }
}