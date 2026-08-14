using NEXT.Features;
using NEXT.Options.Modifiers;
using NEXT.Options.Modifiers.Crewmate;
using NEXT.Utilities;
using MiraAPI.GameOptions;
using MiraAPI.Networking;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using MiraAPI.Modifiers.Types;
using UnityEngine;
using NEXT.Roles;

namespace NEXT.Modifiers.Game.Crewmate;

public class BurstModifier : GameModifier, IModifierDescription
{
    public override string ModifierName => "Burst";
    public override LoadableAsset<Sprite>? ModifierIcon => LaunchpadAssets.BurstIcon;
    public override Color FreeplayFileColor => new Color32(255, 77, 77, 255);

    public string WikiDescription => $"<color=#{LaunchpadPalette.Burst.ToHtmlStringRGBA()}>Burst</color>:\n"+
                                      "When you are killed, you will explode. Which can lead to unexpected deaths around the map.\n"+
                                     $"The explosion radius is {OptionGroupSingleton<BurstOptions>.Instance.BurstRadius}x.";

    public override string GetDescription() =>
        "When you die, players near you also explode!";

    public override int GetAssignmentChance() =>
        (int)OptionGroupSingleton<CrewmateModifierOptions>.Instance.BurstChance;

    public override int GetAmountPerGame() =>
        (int)OptionGroupSingleton<BurstOptions>.Instance.BurstAmount;
    
    public override bool IsModifierValidOn(RoleBehaviour role)
    {
        return base.IsModifierValidOn(role) && role.IsCrewmate();
    }

    public override void OnDeath(DeathReason deathReason)
    {
        if (deathReason != DeathReason.Kill) return;

        var killer = Utils.GetKiller(Player);
        if (killer == null) return;

        float radius = OptionGroupSingleton<BurstOptions>.Instance.BurstRadius;
        Vector2 myPos = Player.GetTruePosition();

        foreach (var target in PlayerControl.AllPlayerControls)
        {
            if (target == null || target.PlayerId == Player.PlayerId) continue;
            if (target.Data.IsDead || target.Data.Disconnected) continue;

            if (Vector2.Distance(myPos, target.GetTruePosition()) > radius) continue;

            killer.RpcCustomMurder(
                target,
                createDeadBody: true,
                didSucceed: true,
                showKillAnim: false,
                playKillSound: true,
                teleportMurderer: false
            );
        }
    }
}