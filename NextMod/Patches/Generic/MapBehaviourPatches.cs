using HarmonyLib;
using NEXT.Utilities;

namespace NEXT.Patches.Generic;

[HarmonyPatch(typeof(MapBehaviour))]
public class MapBehaviourPatches
{
    /// <summary>
    /// Only show map if click is not cancelled
    /// </summary>
    [HarmonyPatch(nameof(MapBehaviour.Show))]
    public static bool Prefix()
    {
        return !Helpers.ShouldCancelClick();
    }
}