using TORWL.Roles.Coven;
using TORWL.Features;
using TORWL.Options.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using MiraAPI.Utilities;
using TORWL.Managers;
using UnityEngine;

namespace TORWL.Buttons.Coven;

public class HexButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Hex";
    public override Color TextOutlineColor => LaunchpadPalette.HexbinderColor;
    public override float Cooldown => OptionGroupSingleton<HexbinderOptions>.Instance.HexCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)OptionGroupSingleton<HexbinderOptions>.Instance.Uses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.Hex;
    public override bool TimerAffectedByPlayer => true;

    public override bool Enabled(RoleBehaviour? role) => role is HexbinderRole;
    
    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);

        Button!.usesRemainingSprite.sprite = LaunchpadAssets.Player.LoadAsset();
        Button!.usesRemainingSprite.color = LaunchpadPalette.HexbinderColor;
    }

    public override PlayerControl? GetTarget() =>
        PlayerControl.LocalPlayer.GetClosestPlayer(true, Distance, false);

    public override void SetOutline(bool active) =>
        Target?.cosmetics.SetOutline(active, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.HexbinderColor));

    public override bool IsTargetValid(PlayerControl? target) => true;

    protected override void OnClick()
    {
        if (Target == null) return;
        HexbinderManager.HexPlayer(Target);
    }
}