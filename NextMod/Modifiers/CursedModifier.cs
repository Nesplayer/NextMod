using System;
using NEXT.Options.Roles.Impostor;
using NEXT.Components;
using NEXT.Features;
using NEXT.Utilities;
using NEXT.Features.Managers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities.Assets;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.Modifiers;
using UnityEngine;

namespace NEXT.Modifiers;

public class CursedModifier : TimedModifier
{
    public override string ModifierName => "Cursed";
    public virtual bool CanReport => false;

    public override string GetDescription() => $"You are cursed for: " +
                                               $"{Math.Round(TimeRemaining, 0)}s/{OptionGroupSingleton<PoltergeistOptions>.Instance.CurseDuration}s";

    public override LoadableAsset<Sprite>? ModifierIcon => LaunchpadAssets.Cursed;

    public override float Duration => OptionGroupSingleton<PoltergeistOptions>.Instance.CurseDuration;

    private readonly PlayerTag _cursedTag = new()
    {
        Name = "CursedTag",
        Text = "Cursed",
        Color = LaunchpadPalette.PoltergeistColor,
        IsLocallyVisible = _ => true,
    };

    public override void OnActivate()
    {
        HudManager.Instance.ReportButton.Hide();

        var tagManager = Player!.GetTagManager();

        if (tagManager != null)
        {
            var existingTag = tagManager.GetTagByName(_cursedTag.Name);
            if (existingTag.HasValue)
            {
                tagManager.RemoveTag(existingTag.Value);
            }

            tagManager.AddTag(_cursedTag);
        }

        NotifyOfCursed();
    }

    [RegisterEvent]
    public static void AfterMurderEvent(AfterMurderEvent e)
    {
        var local = PlayerControl.LocalPlayer;
        if (local == null)
            return;

        var cursed = local.GetModifierComponent()?.GetModifier<CursedModifier>();
        if (cursed == null)
            return;

        if (e.DeadBody == null)
            return;

        var passiveButton = e.DeadBody.GetComponent<PassiveButton>();
        if (passiveButton != null)
            passiveButton.enabled = false;
    }

    public override void OnDeactivate()
    {
        HudManager.Instance.ReportButton.Show();

        var tagManager = Player?.GetTagManager();

        if (tagManager != null)
        {
            tagManager.RemoveTag(_cursedTag);
        }

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p == null || p == Player) continue;

            RestoreIdentity(p);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Player != null)
        {
            PlayerControl.LocalPlayer.lightSource.viewDistance *= 0.5f;
            Player.MyPhysics.body.velocity *= new Vector2(-1, -1);
        }

        var local = PlayerControl.LocalPlayer;

        if (local == null || local != Player)
            return;

        foreach (var p in PlayerControl.AllPlayerControls)
        {
            if (p == null || p == local)
                continue;

            ApplyHiddenIdentity(p);
        }
    }

    private void ApplyHiddenIdentity(PlayerControl target)
    {
        // Disable gradient + force gray
        GradientManager.SetGradientEnabled(target, false);
        target.cosmetics.SetColor(15); // Gray

        // Hide pet
        if (target.cosmetics.CurrentPet != null)
            target.cosmetics.CurrentPet.gameObject.SetActive(false);

        // Hide ALL cosmetics (hat, skin, visor)
        if (target.cosmetics.gameObject.activeSelf)
            target.cosmetics.gameObject.SetActive(false);

        // Random name
        var randomString = MiraAPI.Utilities.Helpers.RandomString(
            Helpers.Random.Next(4, 6),
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#!?$(???#@)$@@@@0000"
        );

        target.cosmetics.SetName(randomString);
        target.cosmetics.SetNameMask(true);
    }

    private void RestoreIdentity(PlayerControl target)
    {
        // Restore gradient
        GradientManager.SetGradientEnabled(target, true);

        // Restore original color
        target.cosmetics.SetColor((byte)target.Data.DefaultOutfit.ColorId);

        // Restore pet
        if (target.cosmetics.CurrentPet != null)
            target.cosmetics.CurrentPet.gameObject.SetActive(true);

        // Restore all cosmetics
        target.cosmetics.gameObject.SetActive(true);

        // Restore real name
        target.SetName(target.Data.PlayerName);
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }

    public void NotifyOfCursed()
    {
        MiraAPI.Utilities.Helpers.CreateAndShowNotification(
            $"{Player.Data.PlayerName} has been cursed by the Poltergeist!",
            LaunchpadPalette.PoltergeistColor,
            null,
            LaunchpadAssets.Cursed.LoadAsset()
        ).transform.localPosition = new Vector3(0f, 1f, -20f);
    }
}