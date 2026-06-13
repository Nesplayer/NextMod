using HarmonyLib;
using MiraAPI.GameOptions;
using MiraAPI.Hud;
using MiraAPI.Modifiers;
using TORWL.Buttons;
using TORWL.Modifiers.Game.Coven;
using TORWL.Options.Modifiers.Coven;

namespace TORWL.Patches;

[HarmonyPatch(typeof(CustomActionButton), nameof(CustomActionButton.ClickHandler))]
public static class RuneBoundPatch
{
    [HarmonyPostfix]
    public static void Postfix(CustomActionButton __instance)
    {
        UnityEngine.Debug.Log($"[RuneBound] ClickHandler postfix fired. Timer={__instance.Timer}");

        if (__instance.EffectActive) return;
        
        var player = PlayerControl.LocalPlayer;
        if (player == null) return;

        UnityEngine.Debug.Log($"[RuneBound] HasModifier check: {player.HasModifier<RuneBoundModifier>()}");

        if (!player.HasModifier<RuneBoundModifier>()) return;

        var reduction = OptionGroupSingleton<RuneBoundOptions>.Instance.CooldownReduction / 100f;
        __instance.Timer *= (1f - reduction);

        UnityEngine.Debug.Log($"[RuneBound] Timer after reduction={__instance.Timer}");
    }
}

[HarmonyPatch(typeof(CustomActionButton), nameof(CustomActionButton.FixedUpdateHandler))]
public static class CooldownDecimalPatch
{
    [HarmonyPostfix]
    public static void Postfix(CustomActionButton __instance)
    {
        if (__instance.Button == null) return;
        if (__instance.EffectActive) return;

        if (__instance.Timer is > 0 and < 10f)
        {
            __instance.Button.cooldownTimerText.text = __instance.Timer.ToString("0.0");
            __instance.Button.cooldownTimerText.gameObject.SetActive(true);
        }
    }
}

[HarmonyPatch(typeof(ActionButton), nameof(ActionButton.SetCoolDown))]
public static class KillButtonDecimalPatch
{
    [HarmonyPostfix]
    public static void Postfix(ActionButton __instance, float timer)
    {
        if (__instance.cooldownTimerText == null) return;
        if (timer is > 0 and < 10f)
        {
            __instance.cooldownTimerText.text = timer.ToString("0.0");
            __instance.cooldownTimerText.gameObject.SetActive(true);
        }
    }
}