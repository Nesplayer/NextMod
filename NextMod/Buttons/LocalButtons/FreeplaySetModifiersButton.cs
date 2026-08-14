using Il2CppSystem;
using NEXT.Components;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities.Assets;
using UnityEngine;
using NEXT.Features;
using MiraAPI.Utilities;
using MiraAPI.Hud;

namespace NEXT.Buttons.Crewmate;

public class SetModifierButton : BaseLaunchpadButton<PlayerControl>
{
    public override string Name => "Set Modifier";
    public override float Cooldown => 0;
    public override int MaxUses => 0;
    public override Color TextOutlineColor => new Color32(89, 223, 231, 255);
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.FreeplayModifierButton;
    public override bool TimerAffectedByPlayer => true;
    public override ButtonLocation Location => ButtonLocation.BottomLeft;

    public override bool Enabled(RoleBehaviour? role)
    {
        return PlayerControl.LocalPlayer != null && TutorialManager.InstanceExists;
    }

    public override PlayerControl? GetTarget() => PlayerControl.LocalPlayer.GetClosestPlayer(true, 1.1f);

    public override void SetOutline(bool active)
    {
        Target?.cosmetics.SetOutline(active, new Nullable<Color>(LaunchpadPalette.CrewMenu));
    }

    public override void ClickHandler()
    {
        if (CanClick())
            OnClick();
    }

    protected override void OnClick()
    {
        if (Target == null) return;

        var target = Target;
        var modifierMenu = SetModifierMinigame.Create();
        modifierMenu.Open(
            _ => true,
            selectedModifier =>
            {
                if (selectedModifier == null || target?.Data == null)
                {
                    modifierMenu.Close();
                    return;
                }

                target.RpcAddModifier(selectedModifier.GetType());
                SoundManager.Instance.PlaySound(LaunchpadAssets.MoneySound.LoadAsset(), false, volume: 5);
                modifierMenu.Close();
            }
        );

        ResetTarget();
    }
}