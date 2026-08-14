using NEXT.Options.Modifiers;
using NEXT.Features;
using NEXT.Options.Modifiers.Universal;
using MiraAPI.Utilities.Assets;
using UnityEngine;
using MiraAPI.GameOptions;
using Reactor.Utilities.Extensions;
using NEXT.Roles;

namespace NEXT.Modifiers.Game.Universal;

public sealed class FlashModifier : LPModifier, IModifierDescription
{
    public override string ModifierName => "Flash";
    public override LoadableAsset<Sprite>? ModifierIcon => LaunchpadAssets.FlashIcon;
    public string WikiDescription => $"<color=#{LaunchpadPalette.Flash.ToHtmlStringRGBA()}>Flash</color>:\n"+
                                      "You are faster than the set player speed which can help with getting tasks done faster, escape kills quickly or flee the murder scene.";
    public override int GetAssignmentChance() => (int)OptionGroupSingleton<UniversalModifierOptions>.Instance.FlashChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<FlashOptions>.Instance.FlashAmount;
    public override Color FreeplayFileColor => Color.yellow;

    public override string GetDescription()
    {
        return "You are faster than other players!";
    }
}