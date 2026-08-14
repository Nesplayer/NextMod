using NEXT.Roles.Coven;
using NEXT.Features;
using NEXT.Options.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using MiraAPI.Utilities;
using NEXT.Managers;
using UnityEngine;

namespace NEXT.Buttons.Coven;

public class CureButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Cure";
    public override Color TextOutlineColor => LaunchpadPalette.PoisonerColor;
    public override float Cooldown => 0;
    public override float EffectDuration => 0;
    public override int MaxUses => 0;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.Cure;
    public override bool TimerAffectedByPlayer => false;

    public override bool Enabled(RoleBehaviour? role) =>
        role is PoisonerRole && OptionGroupSingleton<PoisonerOptions>.Instance.CanCure;

    public override PlayerControl? GetTarget() =>
        PlayerControl.LocalPlayer.GetClosestPlayer(true, Distance, false);

    public override void SetOutline(bool active) =>
        Target?.cosmetics.SetOutline(active, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.PoisonerColor));

    public override bool IsTargetValid(PlayerControl? target) =>
        target?.PlayerId == PoisonerManager.PoisonedPlayer;

    protected override void OnClick()
    {
        if (Target == null) return;
        PoisonerManager.CurePlayer();
    }
}