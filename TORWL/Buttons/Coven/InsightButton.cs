using TORWL.Roles.Coven;
using TORWL.Features;
using TORWL.Modifiers;
using TORWL.Options.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using UnityEngine;
using MiraAPI.Utilities.Assets;
using MiraAPI.Utilities;
using TORWL.Roles.Crewmate;
using TORWL.Roles.Impostor;
using TORWL.Roles.Neutral;
using TORWL.Utilities;

namespace TORWL.Buttons.Coven;

public class InsightButton : BaseLaunchpadButton<PlayerControl>
{

    public override string Name => "Insight";
    public override Color TextOutlineColor => LaunchpadPalette.ScribeColor;
    public override float Cooldown => OptionGroupSingleton<ScribeOptions>.Instance.InsightCooldown;
    public override float EffectDuration => 0;
    public override int MaxUses => (int)OptionGroupSingleton<ScribeOptions>.Instance.MaxInsightUses;
    public override LoadableAsset<Sprite> Sprite => LaunchpadAssets.InsightButton;
    public override bool TimerAffectedByPlayer => true;

    public override bool Enabled(RoleBehaviour? role)
    {
        return role is ScribeRole;
    }

    public override void CreateButton(Transform parent)
    {
        base.CreateButton(parent);

        Button!.usesRemainingSprite.sprite = LaunchpadAssets.Player.LoadAsset();
        Button!.usesRemainingSprite.color = LaunchpadPalette.ScribeColor;
    }

    public override PlayerControl? GetTarget()
    {
        return PlayerControl.LocalPlayer.GetClosestPlayer(true, Distance, false);
    }

    public override void SetOutline(bool active)
    {
        Target?.cosmetics.SetOutline(active, new Il2CppSystem.Nullable<Color>(LaunchpadPalette.ScribeColor));
    }

    public override bool IsTargetValid(PlayerControl? target)
    {
        return true;
    }
    
    protected override void OnClick()
    {
        if (Target == null) return;
    
        var role = Target.Data?.Role;
        if (role == null) return;
    
        string display = role switch
        {
            ICrewmateRole cr => Utils.GetCrewmateFactionDisplay(cr),
            IImpostorRole im => Utils.GetImpostorFactionDisplay(im),
            INeutralRole ne  => Utils.GetNeutralFactionDisplay(ne),
            ICovenRole co    => Utils.GetCovenFactionDisplay(co),
            _ => Utils.GetVanillaRoleFaction(role.Role) is TORWLFactions faction
                ? Utils.GetVanillaFactionDisplay(faction)
                : "Unknown"
        };
    
        AlignmentPopup.ShowAbove(Target, display); 
    }
}