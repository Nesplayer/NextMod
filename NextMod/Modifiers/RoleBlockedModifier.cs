using System;
using System.Collections;
using NEXT.Options.Roles.Coven;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using Reactor.Utilities;
using UnityEngine;
using NEXT.Components;
using NEXT.Features;
using NEXT.Utilities;
using Object = UnityEngine.Object;

namespace NEXT.Modifiers;

public class RoleBlockedModifier : TimedModifier
{
    public override string ModifierName => "Role Blocked";

    public override string GetDescription() => $"You are role blocked for " +
                                               $"{Math.Round(TimeRemaining, 0)}s/{OptionGroupSingleton<TavernKeeperOptions>.Instance.RoleBlockDuration}s";

    public override float Duration => OptionGroupSingleton<TavernKeeperOptions>.Instance.RoleBlockDuration;
    
    private readonly PlayerTag _roleBlockedTag = new()
    {
        Name = "RoleBlockedTag",
        Text = "Role Blocked",
        Color = LaunchpadPalette.TavernKeeperColor,
        IsLocallyVisible = _ => true,
    };

    public override void OnMeetingStart()
    {
        Player.RpcRemoveModifier<RoleBlockedModifier>();
    }

    public override void OnActivate()
    {
        var tagManager = Player!.GetTagManager();

        if (tagManager != null)
        {
            var existingTag = tagManager.GetTagByName(_roleBlockedTag.Name);
            if (existingTag.HasValue)
            {
                tagManager.RemoveTag(existingTag.Value);
            }

            tagManager.AddTag(_roleBlockedTag);
        }
        
        if (Player == PlayerControl.LocalPlayer)
        {
            Coroutines.Start(coroutine: DoAnimation(true));
            PlayerControl.LocalPlayer.cosmetics.SetOutline(true, new Il2CppSystem.Nullable<Color>(new Color(1f, 0f, 1f, 1f)));
        }
    }
    public IEnumerator DoAnimation(bool Show)
    {
        foreach (ActionButton button in Object.FindObjectsOfType<ActionButton>())
        {
            if (Show)
            {
                HudManager.Instance.StartCoroutine(Effects.ScaleIn(button.transform, 0.7f, 0f, 1.5f));

                yield return HudManager.Instance.StartCoroutine(Effects.ColorFade(button.graphic, Palette.White, Palette.Black, 1f));
                button.SetDisabled();
            }
            if (!Show)
            {
                button.SetEnabled();
                HudManager.Instance.StartCoroutine(Effects.ScaleIn(button.transform, 0f, 0.7f, 1.5f));

                HudManager.Instance.StartCoroutine(Effects.ColorFade(button.graphic, Palette.Black, Palette.White, 1f));
            }
        }
        yield break;
    }
    public override void OnDeactivate()
    {
        var tagManager = Player?.GetTagManager();

        if (tagManager != null)
        {
            tagManager.RemoveTag(_roleBlockedTag);
        }
        
        if (Player == PlayerControl.LocalPlayer)
        {
            Coroutines.Start(DoAnimation(false));
            PlayerControl.LocalPlayer.cosmetics.SetOutline(false, new Il2CppSystem.Nullable<Color>(new Color(1f, 0f, 1f, 1f)));
        }
    }
}