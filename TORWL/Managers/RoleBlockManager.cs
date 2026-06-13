using System.Collections;
using System.Collections.Generic;
using Reactor.Utilities;
using UnityEngine;

namespace TORWL.Managers;

public static class RoleblockManager
{
    private static readonly HashSet<byte> _roleblocked = new();

    public static void RoleblockPlayer(byte playerId, float duration)
    {
        _roleblocked.Add(playerId);
        Coroutines.Start(RemoveAfter(playerId, duration));
    }

    public static bool IsRoleblocked(byte playerId) =>
        _roleblocked.Contains(playerId);

    public static void Reset() => _roleblocked.Clear();

    private static IEnumerator RemoveAfter(byte playerId, float duration)
    {
        yield return new WaitForSeconds(duration);
        _roleblocked.Remove(playerId);
    }
}