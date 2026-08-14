using System.Collections.Generic;
using HarmonyLib;

namespace NEXT.Managers;

public static class ShroudweaverManager
{
    private static readonly HashSet<byte> _shroudedPlayers = new();

    public static void ShroudPlayer(PlayerControl target) =>
        _shroudedPlayers.Add(target.PlayerId);

    public static bool IsShrouded(byte playerId) =>
        _shroudedPlayers.Contains(playerId);

    public static void Reset() => _shroudedPlayers.Clear();

    // Clear shrouds at meeting start
    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    public static class MeetingStartPatch
    {
        public static void Postfix() => Reset();
    }

    // Patch Scientist vitals to show shrouded players as dead
    [HarmonyPatch(typeof(VitalsMinigame), nameof(VitalsMinigame.Update))]
    public static class VitalsPatch
    {
        public static void Postfix(VitalsMinigame __instance)
        {
            foreach (var panel in __instance.vitals)
            {
                var data = panel.PlayerInfo;
                if (data == null) continue;
                if (IsShrouded(data.PlayerId))
                {
                    panel.SetDead();
                }
            }
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
    public static class GameStartPatch
    {
        public static void Postfix() => Reset();
    }
}