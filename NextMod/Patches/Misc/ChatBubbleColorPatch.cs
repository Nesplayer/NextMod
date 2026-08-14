using System;
using HarmonyLib;
using MiraAPI.GameOptions;
using NEXT.Options;

namespace NEXT.Patches.Misc
{
	// Token: 0x0200001A RID: 26
	[HarmonyPatch]
	public class ChatBubbleColorPatches
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00003820 File Offset: 0x00001A20
		[HarmonyPatch(typeof(ChatBubble), "SetText")]
		[HarmonyPostfix]
		public static void ChatBubbleSetTextPostfix(ChatBubble __instance)
		{
			if (OptionGroupSingleton<FunOptions>.Instance.ColorfulBubbles)
			{
				__instance.Background.color = __instance.playerInfo.Color;
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003849 File Offset: 0x00001A49
		[HarmonyPatch(typeof(ChatNotification), "SetUp")]
		[HarmonyPostfix]
		public static void ChatNotificationSetUpPostfix(ChatNotification __instance, ref PlayerControl sender)
		{
			if (OptionGroupSingleton<FunOptions>.Instance.ColorfulBubbles)
			{
				__instance.background.color = sender.Data.Color;
			}
		}
	}
}