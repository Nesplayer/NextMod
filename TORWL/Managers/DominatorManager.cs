using HarmonyLib;
using MiraAPI.Roles;
using UnityEngine;

namespace TORWL.Roles.Coven;

public static class DominatorManager
{
    public static byte? DominatedPlayer { get; private set; }

    public static void DominatePlayer(PlayerControl target)
    {
        DominatedPlayer = target.PlayerId;

        var role = target.Data?.Role;
        switch (role)
        {
            case EngineerRole:
                Vent? closestVent = null;
                float closestDist = float.MaxValue;
                foreach (var vent in ShipStatus.Instance.AllVents)
                {
                    var dist = Vector2.Distance(target.transform.position, vent.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestVent = vent;
                    }
                }
                if (closestVent != null)
                    target.MyPhysics.RpcEnterVent(closestVent.Id);
                break;

            case ScientistRole scientist:
                if (scientist.VitalsPrefab != null)
                {
                    var minigame = Object.Instantiate(scientist.VitalsPrefab);
                    minigame.transform.SetParent(Camera.main.transform, false);
                    minigame.transform.localPosition = new Vector3(0f, 0f, -50f);
                    minigame.Begin(null);
                }
                break;

            case ICustomRole when role is IDominateRole dominateRole:
                dominateRole.OnDominated(target);
                break;
        }

        DominatedPlayer = null;
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
    public static class GameStartPatch
    {
        public static void Postfix() => DominatedPlayer = null;
    }
}