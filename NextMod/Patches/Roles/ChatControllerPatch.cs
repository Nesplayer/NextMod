using HarmonyLib;
using NEXT.Modifiers;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using TMPro;
using Object = UnityEngine.Object;

namespace NEXT.Patches.Roles;

[HarmonyPatch(typeof(ChatController))]
public static class ChatControllerPatches
{
    private static TextMeshPro? _noticeText;

    [HarmonyPostfix]
    [HarmonyPatch(nameof(ChatController.Awake))]
    public static void AwakePostfix(ChatController __instance)
    {
        _noticeText =
            Object.Instantiate(__instance.sendRateMessageText, __instance.sendRateMessageText.transform.parent);
        _noticeText.text = string.Empty;
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(ChatController.UpdateChatMode))]
    public static void UpdatePostfix(ChatController __instance)
    {
        if (_noticeText == null || !PlayerControl.LocalPlayer)
        {
            return;
        }

        if (!MeetingHud.Instance)
        {
            if (_noticeText.text != string.Empty)
            {
                _noticeText.text = string.Empty;
            }

            return;
        }

        if (PlayerControl.LocalPlayer.HasModifier<SilencedModifier>() &&
            !PlayerControl.LocalPlayer.Data.IsDead)
        {
            _noticeText.text = "You have been Silenced.";
            __instance.freeChatField.SetVisible(false);
            __instance.quickChatField.SetVisible(false);
        }
    }
}