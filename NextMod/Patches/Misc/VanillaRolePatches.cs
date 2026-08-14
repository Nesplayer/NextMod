using AmongUs.GameOptions;
using HarmonyLib;
using MiraAPI.Roles;
using NEXT;
using NEXT.Features;
using UnityEngine;

namespace NEXT.Patches.Roles;

[HarmonyPatch(typeof(RoleBehaviour))]
public static class VanillaRolePatches
{
    [HarmonyPatch(nameof(RoleBehaviour.RoleIconSolid), MethodType.Getter)]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.First)]
    public static bool VanillaIconPrefix(RoleBehaviour __instance, ref Sprite __result)
    {
        if (__instance.IsCustomRole())
            return true;

        var icon = TryGetVanillaRoleIcon(__instance.Role);
        if (icon == null)
            return true;

        __result = icon;
        return false;
    }

    private static Sprite? TryGetVanillaRoleIcon(RoleTypes role)
    {
        return role switch
        {
            RoleTypes.Crewmate => LaunchpadAssets.CrewIcon.LoadAsset(),
            RoleTypes.Impostor => LaunchpadAssets.ImpIcon.LoadAsset(),
            _                  => null
        };
    }
}