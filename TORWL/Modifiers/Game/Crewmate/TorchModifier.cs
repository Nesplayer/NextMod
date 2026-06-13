using TORWL.Options.Modifiers;
using TORWL.Features;
using TORWL.Options.Modifiers.Crewmate;
using TORWL.Utilities;
using MiraAPI.GameOptions;
using MiraAPI.Utilities.Assets;
using UnityEngine;
using Reactor.Utilities.Extensions;
using MiraAPI.Modifiers.Types;
using TORWL.Roles;

namespace TORWL.Modifiers.Game.Crewmate;

public sealed class TorchModifier : GameModifier, IModifierDescription
{
    public override string ModifierName => "Torch";
    public override Color FreeplayFileColor => new Color32(255, 127, 50, 255);
    public override LoadableAsset<Sprite>? ModifierIcon => LaunchpadAssets.TorchIcon;

    public string WikiDescription => $"<color=#{LaunchpadPalette.Torch.ToHtmlStringRGBA()}>Torch</color>:\n"+
                                     (OptionGroupSingleton<TorchOptions>.Instance.UseFlashlight
                                     ? $"You will have a flashlight if lights are sabotaged. It works exactly like the Hide and Seek flaslight so that you can see in the dark which can help finding the <color=#{LaunchpadPalette.Impostor.ToHtmlStringRGBA()}>Impostors</color> when they kill someone during the lights sabotage."
                                     : $"You have max vision if lights are sabotaged, which can help finding the <color=#{LaunchpadPalette.Impostor.ToHtmlStringRGBA()}>Impostors</color> when they kill someone during the lights sabotage.");
    public override string GetDescription() => 
        OptionGroupSingleton<TorchOptions>.Instance.UseFlashlight
        ? "You will have a flashlight\nif lights are sabotaged."
        : "You have max vision\nif lights are sabotaged.";

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.TorchChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<TorchOptions>.Instance.TorchAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }
}