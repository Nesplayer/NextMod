using NEXT.Options.Modifiers;
using NEXT.Options.Modifiers.Crewmate;
using NEXT.Features;
using NEXT.Utilities;
using MiraAPI.Utilities.Assets;
using MiraAPI.GameOptions;
using MiraAPI.Utilities;
using Reactor.Utilities.Extensions;
using UnityEngine;
using MiraAPI.Modifiers.Types;
using NEXT.Roles;

namespace NEXT.Modifiers.Game.Crewmate;

public sealed class MayorModifier : GameModifier, IModifierDescription
{
    public override string ModifierName => "Mayor";
    public override LoadableAsset<Sprite>? ModifierIcon => LaunchpadAssets.MayorIcon;
    public override Color FreeplayFileColor => new Color32(155, 89, 182, 255);

    public string WikiDescription => $"<color=#{LaunchpadPalette.Mayor.ToHtmlStringRGBA()}>Mayor</color>:\n"+
                                     $"You have an additional {OptionGroupSingleton<MayorOptions>.Instance.ExtraVotes} votes every meeting."+
                                     "This can help with some extra votes needed so that nothing could possibly end with a tie.";

    public override string GetDescription() =>
        $"You have an additional {OptionGroupSingleton<MayorOptions>.Instance.ExtraVotes} votes every meeting.";

    public override int GetAssignmentChance() => (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.MayorChance;
    public override int GetAmountPerGame() => (int)OptionGroupSingleton<MayorOptions>.Instance.MayorAmount;

    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }

    public override void OnMeetingStart()
    {
        var voteData = Player.GetVoteData();
        if (!voteData) return;

        voteData.VotesRemaining += (int)OptionGroupSingleton<MayorOptions>.Instance.ExtraVotes;
    }
}