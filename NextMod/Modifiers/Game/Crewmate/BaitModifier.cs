using NEXT.Features;
using NEXT.Options.Modifiers;
using NEXT.Options.Modifiers.Crewmate;
using MiraAPI.Utilities.Assets;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using UnityEngine;
using Reactor.Utilities.Extensions;
using NEXT.Utilities;
using NEXT.Roles;

namespace NEXT.Modifiers.Game.Crewmate;

public class BaitModifier : GameModifier, IModifierDescription
{
    public override string ModifierName => "Bait";
    public override LoadableAsset<Sprite>? ModifierIcon => LaunchpadAssets.BaitIcon;
    public override Color FreeplayFileColor => LaunchpadPalette.Bait;

    public string WikiDescription => $"<color=#{LaunchpadPalette.Bait.ToHtmlStringRGBA()}>Bait</color>:\n"+
                                      "When you are killed, the player that killed you will automatically self-report your body.";

    public override int GetAmountPerGame()
    {
        return (int)OptionGroupSingleton<BaitOptions>.Instance.BaitAmount;
    }

    public override int GetAssignmentChance()
    {
        return (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.BaitChance;
    }

    public override string GetDescription()
    {
        return "Your killer self-reports after you are killed!";
    }

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }

    [RegisterEvent]
    public static void OnKill(AfterMurderEvent e)
    {
        if (ModifierExtensions.HasModifier<BaitModifier>(e.Target))
        {
            e.Source.CmdReportDeadBody(e.Target.Data);
        }
    }
}