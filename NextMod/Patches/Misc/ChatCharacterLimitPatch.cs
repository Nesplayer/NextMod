using HarmonyLib;
using UnityEngine;

namespace NEXT.Patches;

[HarmonyPatch(typeof(ChatController), nameof(ChatController.Update))]
public static class ChatCharacterLimitPatch
{
    public static void Postfix(ChatController __instance)
    {
        if (__instance.freeChatField?.textArea != null)
            __instance.freeChatField.textArea.characterLimit = 2000;
    }
}

[HarmonyPatch(typeof(FreeChatInputField), nameof(FreeChatInputField.UpdateCharCount))]
public static class ChatCharCountPatch
{
    public static void Postfix(FreeChatInputField __instance)
    {
        var length = __instance.textArea.text.Length;
        __instance.charCountText.SetText($"{length}/{__instance.textArea.characterLimit}");

        __instance.charCountText.color = length switch
        {
            // White under 75% (150 chars)
            < 1500 => Color.black,
            // Yellow when close (150�199)
            < 2000 => new Color(1f, 1f, 0f, 1f),
            // Red at limit
            _ => Color.red
        };
    }
}