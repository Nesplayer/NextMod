using HarmonyLib;
using NEXT.Utilities;

namespace NEXT.Patches.Colors.Gradients;

[HarmonyPatch(typeof(PlayerVoteArea),nameof(PlayerVoteArea.SetCosmetics))]
public static class PlayerVoteAreaSetCosmeticsPatch
{
    public static void Prefix(PlayerVoteArea __instance, [HarmonyArgument(0)] NetworkedPlayerInfo playerInfo)
    {
        __instance.PlayerIcon.gameObject.SetGradientData(playerInfo.PlayerId);
    }
}