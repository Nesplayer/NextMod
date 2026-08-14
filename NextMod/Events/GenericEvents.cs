using System;
using System.Linq;
using NEXT.Buttons.Impostor;
using NEXT.Components;
using NEXT.Features;
using NEXT.GameOver;
using NEXT.Modifiers;
using NEXT.Options.Roles.Crewmate;
using NEXT.Roles.Crewmate;
using NEXT.Roles.Impostor;
using NEXT.Roles.Neutral;
using NEXT.Utilities;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Events.Vanilla.Meeting;
using MiraAPI.Events.Vanilla.Usables;
using MiraAPI.GameEnd;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using MiraAPI.Roles;

namespace NEXT.Events;

public static class GenericEvents
{
    [RegisterEvent]
    public static void SetRoleEvent(SetRoleEvent @event)
    {
        if (@event.Player.AmOwner && NotepadHud.Instance != null)
        {
            NotepadHud.Instance.UpdateAspectPos();
        }

        var tagManager = @event.Player.GetTagManager();

        if (tagManager == null)
        {
            return;
        }

        var existingRoleTag = tagManager.GetTagByName("Role");
        if (existingRoleTag.HasValue)
        {
            tagManager.RemoveTag(existingRoleTag.Value);
        }

        var role = @event.Player.Data.Role;
        var color = role is ICustomRole custom ? custom.RoleColor : role.TeamColor;
        var name = role.NiceName;

        if (role.IsDead && name == "STRMISS")
        {
            name = "Ghost";
        }

        var roleTag = new PlayerTag
        {
            Name = "Role",
            Text = name,
            Color = color,
            IsLocallyVisible = (player) =>
            {
                var plrRole = player.Data.Role;

                if (player.HasModifier<RevealedModifier>())
                {
                    return true;
                }

                if (plrRole is ICustomRole customRole && (player.AmOwner || customRole.CanLocalPlayerSeeRole(player)))
                {
                    return true;
                }

                if (player.AmOwner || PlayerControl.LocalPlayer.Data.IsDead)
                {
                    return true;
                }

                return false;
            },
        };

        tagManager.AddTag(roleTag);
    }

    [RegisterEvent]
    public static void EjectEvent(EjectionEvent @event)
    {
        if (NotepadHud.Instance != null)
        {
            NotepadHud.Instance.UpdateAspectPos();
        }

        if (@event.ExileController.initData.networkedPlayer != null && @event.ExileController.initData.networkedPlayer.Role != null
                                                        && @event.ExileController.initData.networkedPlayer.Role is JesterRole)
        {
            CustomGameOver.Trigger<JesterGameOver>([@event.ExileController.initData.networkedPlayer]);
        }

        foreach (var plr in PlayerControl.AllPlayerControls)
        {
            var tagManager = plr.GetTagManager();
            if (tagManager != null)
            {
                tagManager.MeetingEnd();
            }
        }

        foreach (var body in DeadBodyCacheComponent.GetFrozenBodies())
        {
            body.body.hideFlags = UnityEngine.HideFlags.None;
        }
    }

    [RegisterEvent]
    public static void ReportBodyEvent(ReportBodyEvent bodyEvent)
    {
        if (bodyEvent.Reporter.HasModifier<DragBodyModifier>())
        {
            bodyEvent.Cancel();
            return;
        }

        if (PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>())
        {
            PlayerControl.LocalPlayer.RpcRemoveModifier<DragBodyModifier>();
        }

        if (PlayerControl.LocalPlayer.Data.Role is SwapshifterRole swap && CustomButtonSingleton<SwapButton>.Instance.EffectActive)
        {
            CustomButtonSingleton<SwapButton>.Instance.OnEffectEnd();
        }

        if (PlayerControl.LocalPlayer.Data.Role is HitmanRole { inDeadlockMode: true } && HitmanUtilities.MarkedPlayers != null)
        {
            HitmanUtilities.ClearMarks();
            CustomButtonSingleton<DeadlockButton>.Instance.OnEffectEnd();
        }
    }

    [RegisterEvent(-10)]
    public static void CanUseEvent(PlayerCanUseEvent @event)
    {
        if (!PlayerControl.LocalPlayer)
        {
            return;
        }

        if (PlayerControl.LocalPlayer.HasModifier<DragBodyModifier>())
        {
            if (!@event.IsVent)
            {
                @event.Cancel();
            }

            return;
        }

        if (@event.IsVent)
        {
            var vent = @event.Usable.Cast<Vent>();
            if (vent.IsSealed())
            {
                @event.Cancel();
                return;
            }
        }
    }
}