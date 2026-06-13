using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TORWL.Features.Outbreak;

/// <summary>
/// Renders per-player infection progress by tinting the target's name
/// from white → red as infection builds up (0 → 1).
/// Also shows a small ☣ proximity warning on the local player's screen
/// when an infected player is within range.
/// </summary>
public static class OutbreakHud
{
    private static bool _visible;

    // Maps PlayerId → current infection progress (0..1), received via RPC
    private static readonly Dictionary<byte, float> _progress = new();

    // Optional top-centre status label
    private static GameObject? _hudRoot;
    private static TextMeshPro? _statusLabel;

    private const string StatusText = "<color=#32CD32>☣ OUTBREAK ACTIVE</color>";

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    public static void SetVisible(bool visible)
    {
        _visible = visible;
        if (visible) EnsureCreated();
        if (_hudRoot != null) _hudRoot.SetActive(visible);
        if (!visible) ClearAllProgress();
    }

    public static void SetInfectionProgress(byte playerId, float progress)
    {
        _progress[playerId] = progress;
    }

    public static void ClearAllProgress()
    {
        _progress.Clear();
        // Restore all name colours
        foreach (var pc in PlayerControl.AllPlayerControls)
        {
            if (pc?.cosmetics?.nameText == null) continue;
            pc.cosmetics.nameText.color = Color.white;
        }
    }

    // ── Per-frame update ──────────────────────────────────────────────────────

    /// <summary>
    /// Called from OutbreakPatches.HudUpdate_Postfix every frame.
    /// Applies name tinting based on current infection progress.
    /// </summary>
    public static void Tick()
    {
        if (!_visible) return;

        foreach (var pc in PlayerControl.AllPlayerControls)
        {
            if (pc == null || pc.cosmetics?.nameText == null) continue;

            if (_progress.TryGetValue(pc.PlayerId, out var t) && t > 0f)
            {
                // Lerp white → red
                pc.cosmetics.nameText.color = Color.Lerp(Color.white, new Color(1f, 0.1f, 0.1f), t);
            }
            else
            {
                pc.cosmetics.nameText.color = Color.white;
            }
        }
    }

    // ── Internal ──────────────────────────────────────────────────────────────

    private static void EnsureCreated()
    {
        if (_hudRoot != null) return;

        var hud = HudManager.Instance;
        if (hud == null) return;

        _hudRoot = new GameObject("TORWL_OutbreakHud");
        _hudRoot.transform.SetParent(hud.transform, false);
        _hudRoot.transform.localPosition = new Vector3(0f, 2.3f, -5f);

        _statusLabel = _hudRoot.AddComponent<TextMeshPro>();
        _statusLabel.text = StatusText;
        _statusLabel.fontSize = 3f;
        _statusLabel.alignment = TextAlignmentOptions.Center;
        _statusLabel.enableWordWrapping = false;
        _statusLabel.overflowMode = TextOverflowModes.Overflow;
        _statusLabel.richText = true;
        _statusLabel.sortingOrder = 100;

        _hudRoot.SetActive(false);
    }
}
