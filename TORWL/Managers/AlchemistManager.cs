using System;
using System.Collections;
using HarmonyLib;
using MiraAPI.GameOptions;
using Reactor.Utilities;
using TMPro;
using TORWL.Options.Roles.Coven;
using TORWL.Roles.Coven;
using TORWL.Utilities;
using UnityEngine;
using Reactor.Utilities.Extensions;

namespace TORWL.Managers;

public enum PotionType
{
    Roleblock,
    SpeedBoost,
    Kill
}

public static class AlchemistManager
{
    private static readonly System.Random _rng = new();
    private const string PotionLabelTag = "PotionLabel";

    public static void ThrowPotion(PlayerControl target)
    {
        var opts = OptionGroupSingleton<AlchemistOptions>.Instance;

        var maxPotion = opts.CanBrewKill ? 3 : 2;
        var potion = (PotionType)_rng.Next(0, maxPotion);

        switch (potion)
        {
            case PotionType.Roleblock:
                ApplyRoleblock(target, opts.EffectDuration);
                ShowPotionLabel(target, "Roleblock");
                break;
            case PotionType.SpeedBoost:
                ApplySpeedBoost(target, opts.EffectDuration);
                ShowPotionLabel(target, "Speed Boost");
                break;
            case PotionType.Kill:
                target.Exiled();
                break;
        }
    }

    private static void ShowPotionLabel(PlayerControl target, string potionName)
    {
        // Show to the target
        if (target == PlayerControl.LocalPlayer)
            ShowLabel(target, potionName);

        // Show to the local alchemist
        if (PlayerControl.LocalPlayer?.Data?.Role is AlchemistRole)
            ShowLabel(target, potionName);
    }

    private static void ShowLabel(PlayerControl player, string potionName)
    {
        if (player?.cosmetics?.nameTextContainer == null) return;

        var existing = player.cosmetics.nameTextContainer.transform.FindChild(PotionLabelTag);
        if (existing != null) existing.gameObject.Destroy();

        var go = new GameObject(PotionLabelTag);
        go.transform.SetParent(player.cosmetics.nameTextContainer.transform, false);
        go.transform.localPosition = new Vector3(0f, 0.35f, -1f);

        var tmp = go.AddComponent<TextMeshPro>();
        tmp.text = $"<color=#9B59B6><b>Received the {potionName}!</b></color>";
        tmp.fontSize = 1.8f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.richText = true;

        Coroutines.Start(FadeOutLabel(go, 5.5f));
    }

    private static IEnumerator FadeOutLabel(GameObject go, float duration)
    {
        if (go == null) yield break;

        var tmp = go.GetComponent<TextMeshPro>();
        float elapsed = 0f;
        float fadeStart = duration * 0.6f;

        while (elapsed < duration)
        {
            if (go == null) yield break;
            elapsed += Time.deltaTime;

            if (elapsed > fadeStart && tmp != null)
            {
                float alpha = 1f - ((elapsed - fadeStart) / (duration - fadeStart));
                var c = tmp.color;
                c.a = alpha;
                tmp.color = c;
            }

            yield return null;
        }

        if (go != null) go.Destroy();
    }

    private static void ApplyRoleblock(PlayerControl target, float duration)
    {
        RoleblockManager.RoleblockPlayer(target.PlayerId, duration);
    }

    private static void ApplySpeedBoost(PlayerControl target, float duration)
    {
        Coroutines.Start(SpeedBoostCoroutine(target, duration));
    }

    private static IEnumerator SpeedBoostCoroutine(PlayerControl target, float duration)
    {
        var original = target.MyPhysics.TrueSpeed;
        target.MyPhysics.Speed = original * 2f;
        yield return new WaitForSeconds(duration);
        target.MyPhysics.Speed = original;
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
    public static class GameStartPatch
    {
        public static void Postfix() => RoleblockManager.Reset();
    }
}