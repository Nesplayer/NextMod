using NEXT.Roles.Coven;
using NEXT.Features;
using NEXT.Options.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using MiraAPI.Utilities;
using NEXT.Managers;
using UnityEngine;

namespace NEXT.Buttons.Coven;

public class PoisonButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Poison";
    public override Color TextOutlineColor => LaunchpadPalette.PoisonerColor;
    public override float Cooldown => OptionGroupSingleton<PoisonerOptions>.Instance.PoisonCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)OptionGroupSingleton<PoisonerOptions>.Instance.Uses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.Toxify;
    public override bool TimerAffectedByPlayer => true;

    public override bool Enabled(RoleBehaviour? role) => role is PoisonerRole;
    
    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);

        Button!.usesRemainingSprite.sprite = LaunchpadAssets.Player.LoadAsset();
        Button!.usesRemainingSprite.color = LaunchpadPalette.PoisonerColor;
    }

    public override PlayerControl? GetTarget() =>
        PlayerControl.LocalPlayer.GetClosestPlayer(true, Distance, false);

    public override void SetOutline(bool active) =>
        Target?.cosmetics.SetOutline(active, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.PoisonerColor));

    public override bool IsTargetValid(PlayerControl? target) =>
        PoisonerManager.PoisonedPlayer != target?.PlayerId;

    protected override void OnClick()
    {
        if (Target == null) return;
        PoisonerManager.PoisonPlayer(Target);
    }
}