using HarmonyLib;
using NEXT.Utilities;

namespace NEXT.Patches.Roles.Scientist;

[HarmonyPatch(typeof(ScientistRole))]
public static class ScientistPatches
{
    [HarmonyPatch(nameof(ScientistRole.RefreshAbilityButton))]
    public static bool Prefix()
    {

        DestroyableSingleton<HudManager>.Instance.AbilityButton.SetDisabled();
        return false;

    }
}