using TMPro;
using UnityEngine;

namespace NEXT.Features.StopNGo;

/// <summary>
/// Simple HUD overlay that shows the current light state and a countdown timer.
/// </summary>
public static class StopNGoHud
{
    private static GameObject? _hudRoot;
    private static TextMeshPro? _label;
    private static TextMeshPro? _timerLabel;

    private const string GreenText = "<color=#00E676>● GREEN LIGHT</color>";
    private const string RedText = "<color=#FF1744>● RED LIGHT</color>";

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    public static void SetVisible(bool visible)
    {
        if (visible) EnsureCreated();
        if (_hudRoot != null) _hudRoot.SetActive(visible);
    }

    public static void UpdateLight(StopNGoMode.LightState state)
    {
        EnsureCreated();
        if (_label == null) return;
        _label.text = state == StopNGoMode.LightState.Green ? GreenText : RedText;
    }

    /// <summary>
    /// Called every second by the host's light loop to tick the countdown.
    /// secondsLeft = seconds remaining in the current phase.
    /// </summary>
    public static void UpdateTimer(float secondsLeft)
    {
        EnsureCreated();
        if (_timerLabel == null) return;
        _timerLabel.text = $"<color=#FFFFFF>{Mathf.CeilToInt(secondsLeft)}s</color>";
    }

    // ── Internal ──────────────────────────────────────────────────────────────

    private static void EnsureCreated()
    {
        if (_hudRoot != null) return;

        var hud = HudManager.Instance;
        if (hud == null) return;

        _hudRoot = new GameObject("NEXT_StopNGoHud");
        _hudRoot.transform.SetParent(hud.transform, false);
        _hudRoot.transform.localPosition = new Vector3(0f, 2.3f, -5f);

        // Main light label
        _label = _hudRoot.AddComponent<TextMeshPro>();
        _label.text = GreenText;
        _label.fontSize = 3.5f;
        _label.alignment = TextAlignmentOptions.Center;
        _label.enableWordWrapping = false;
        _label.overflowMode = TextOverflowModes.Overflow;
        _label.richText = true;
        _label.sortingOrder = 100;

        // Countdown label — sits just below the main label
        var timerGo = new GameObject("NEXT_StopNGoTimer");
        timerGo.transform.SetParent(_hudRoot.transform, false);
        timerGo.transform.localPosition = new Vector3(0f, -0.55f, 0f);

        _timerLabel = timerGo.AddComponent<TextMeshPro>();
        _timerLabel.text = "";
        _timerLabel.fontSize = 2.5f;
        _timerLabel.alignment = TextAlignmentOptions.Center;
        _timerLabel.enableWordWrapping = false;
        _timerLabel.overflowMode = TextOverflowModes.Overflow;
        _timerLabel.richText = true;
        _timerLabel.sortingOrder = 100;

        _hudRoot.SetActive(false);
    }
}