using TORWL.Components;
using TORWL.Options.Modifiers.Universal;
using TORWL.Options.Modifiers;
using TORWL.Features;
using MiraAPI.Utilities.Assets;
using UnityEngine;
using MiraAPI.GameOptions;
using Reactor.Utilities.Extensions;
using TORWL.Roles;

namespace TORWL.Modifiers.Game.Universal;

public sealed class KingModifier : LPModifier, IModifierDescription
{
    public override string ModifierName => "V.I.P";
    public override Color FreeplayFileColor => new Color32(255, 215, 0, 255);
    public override LoadableAsset<Sprite>? ModifierIcon => LaunchpadAssets.VIPIcon;
    public string WikiDescription => $"<color=#{LaunchpadPalette.VIP.ToHtmlStringRGBA()}>V.I.P</color>:\n"+
                                      "You look fancy, to fool the crews into thinking you are innocent only because you look so fancy.\n\n"+
                                      "<b><i>THIS MODIFIER WILL BE REWORKED INTO SOMETHING BETTER!<i><b>\n\n"+
                                      "<b><i>I plan to make this modifier a Celebrity modifier which is better than V.I.P.</i></b>";
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.KingChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<KingOptions>.Instance.KingAmount;

    public override string GetDescription()
    {
        return "You just look fancy!";
    }

    public override void OnActivate()
    {
        PlayerControl.LocalPlayer.RpcSetHat("hat_NewYear2024");
        PlayerControl.LocalPlayer.RpcSetSkin("skin_Bling");
        PlayerControl.LocalPlayer.RpcSetVisor("visor_masque_white");
    }

    public override void OnDeactivate()
    {
        PlayerControl.LocalPlayer.RpcSetHat("");
        PlayerControl.LocalPlayer.RpcSetSkin("");
        PlayerControl.LocalPlayer.RpcSetVisor("");
    }
}