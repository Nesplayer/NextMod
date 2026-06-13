using MiraAPI.Utilities;
using MiraAPI.Utilities.Assets;
using TORWL.Roles.Crewmate;
using TORWL.Features;
using UnityEngine;
using TORWL.Patches.Roles.Shielder;
using Il2CppSystem;
using TORWL.Options.Roles.Crewmate;
using MiraAPI.GameOptions;

namespace TORWL.Buttons;

public class ShieldButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Shield";
    public override float Cooldown => OptionGroupSingleton<ShielderOptions>.Instance.ShieldCooldown;
    public override int MaxUses => (int)OptionGroupSingleton<ShielderOptions>.Instance.MaxShields;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.ShieldButton;

    public override bool Enabled(RoleBehaviour? role) => role is ShielderRole;

    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);

        Button!.usesRemainingSprite.sprite = LaunchpadAssets.Player.LoadAsset();
        Button!.usesRemainingSprite.color = LaunchpadPalette.MedicColor;
    }

    public override float Distance => PlayerControl.LocalPlayer.MaxReportDistance;

    public override bool TimerAffectedByPlayer => false;


    public override PlayerControl? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestPlayer(true, 1.1f);
    }

    public override void SetOutline(bool active)
    {
        Target?.cosmetics.SetOutline(active, new Nullable<Color>(LaunchpadPalette.ShielderColor));
    }

    public override bool IsTargetValid(PlayerControl? target)
    {
        if (target == null) return false;
        if (target == PlayerControl.LocalPlayer) return false; // no self-shield
        if (target.Data.IsDead) return false;

        // Don't allow placing a second shield on the same person who is already shielded.
        var shielderRole = PlayerControl.LocalPlayer.Data.Role as ShielderRole;
        if (shielderRole?.ShieldedPlayer == target) return false;

        return true;
    }

    // --- On click ---

    protected override void OnClick()
    {
        if (Target == null) return;

        var shielderRole = PlayerControl.LocalPlayer.Data.Role as ShielderRole;
        if (shielderRole == null) return;

        // Remove the previous shield outline before moving it.
        if (shielderRole.ShieldedPlayer != null)
        {
            shielderRole.ShieldedPlayer.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.SheriffColor));
        }

        shielderRole.ShieldedPlayer = Target;

        // Sync state to other clients via RPC so the murder patch works everywhere.
        ShielderRpc.RpcSetShield(PlayerControl.LocalPlayer, Target);
    }
}
