using HarmonyLib;
using NEXT.Components;
using NEXT.Features;
using UnityEngine;

namespace NEXT.Patches.Colors.Gradients;

[HarmonyPatch(typeof(PlayerParticles),nameof(PlayerParticles.PlacePlayer))]
public static class MainMenuPlayerParticlePatch
{
    public static void Postfix(PlayerParticle part)
    {
        part.myRend.material = LaunchpadAssets.GradientMaterial.LoadAsset();
        var id = part.gameObject.AddComponent<PlayerGradientData>().GradientColor;
        part.GetComponent<GradientColorComponent>().SetColor(Random.RandomRangeInt(0,Palette.PlayerColors.Length), id);
    }
}