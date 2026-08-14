using NEXT.Roles.Coven;
using NEXT.Features;
using NEXT.Options.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using MiraAPI.Utilities;
using UnityEngine;

namespace NEXT.Buttons.Coven;

public class DominateButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Dominate";
    public override Color TextOutlineColor => LaunchpadPalette.DominatorColor;
    public override float Cooldown => OptionGroupSingleton<DominatorOptions>.Instance.DominateCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => 1;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.Dominate;
    public override bool TimerAffectedByPlayer => true;

    public override bool Enabled(RoleBehaviour? role) => role is DominatorRole;
    
    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);

        Button!.usesRemainingSprite.sprite = LaunchpadAssets.Player.LoadAsset();
        Button!.usesRemainingSprite.color = LaunchpadPalette.HexbinderColor;
    }

    public override PlayerControl? GetTarget() =>
        PlayerControl.LocalPlayer.GetClosestPlayer(true, Distance, false);

    public override void SetOutline(bool active) =>
        Target?.cosmetics.SetOutline(active, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.DominatorColor));

    public override bool IsTargetValid(PlayerControl? target)
    {
        if (target == null) return false;
        var role = target.Data?.Role;
        return role is EngineerRole or ScientistRole;
    }

    protected override void OnClick()
    {
        if (Target == null) return;
        DominatorManager.DominatePlayer(Target);
    }
}