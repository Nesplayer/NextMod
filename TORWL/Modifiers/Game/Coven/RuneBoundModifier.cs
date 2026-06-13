using TORWL.Features;
using TORWL.Options.Modifiers;
using TORWL.Options.Modifiers.Crewmate;
using TORWL.Utilities;
using MiraAPI.GameOptions;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using MiraAPI.Modifiers.Types;
using TORWL.Buttons;
using TORWL.Options.Modifiers.Coven;
using UnityEngine;
using TORWL.Roles;

namespace TORWL.Modifiers.Game.Coven;

public class RuneBoundModifier : GameModifier, IModifierDescription
{
    public override string ModifierName => "Rune Bound";
    public override LoadableAsset<Sprite>? ModifierIcon => LaunchpadAssets.TavernKeeper;
    public override Color FreeplayFileColor => LaunchpadPalette.Coven;

    public string WikiDescription => $"<color=#{LaunchpadPalette.Coven.ToHtmlStringRGBA()}>Rune Bound</color>:\n"+
                                     $"Your ability cooldown gets decreased by {OptionGroupSingleton<RuneBoundOptions>.Instance.CooldownReduction}%\n"+
                                     $"It can help you use your abilities faster to win before the <color=#{LaunchpadPalette.Impostor.ToHtmlStringRGBA()}>Impostors</color> or <color=#{LaunchpadPalette.Neutral.ToHtmlStringRGBA()}>Neutrals</color> does.";

    public override string GetDescription() =>
        $"Your ability cooldown is decreased by {OptionGroupSingleton<RuneBoundOptions>.Instance.CooldownReduction}%.";

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CovenModifierOptions>.Instance.RuneBoundChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<RuneBoundOptions>.Instance.RBAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCoven();
    }

    public static float ApplyCooldownReduction(float original)
    {
        var reduction = OptionGroupSingleton<RuneBoundOptions>.Instance.CooldownReduction / 100f;
        return original * (1f - reduction);
    }
}