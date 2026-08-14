using HarmonyLib;
using NEXT.Components;

namespace NEXT.Patches.Generic;

[HarmonyPatch(typeof(GameManager), nameof(GameManager.Awake))]
public static class GameManagerPatch
{
    public static void Postfix(GameManager __instance)
    {
        foreach (var deadBody in __instance.deadBodyPrefab)
        {
            deadBody.gameObject.AddComponent<DeadBodyCacheComponent>();
        }
    }
}