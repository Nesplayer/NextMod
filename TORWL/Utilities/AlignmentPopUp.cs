using TMPro;
using UnityEngine;
using Reactor.Utilities;
using Reactor.Utilities.Extensions;

namespace TORWL.Utilities;

public static class AlignmentPopup
{
    public static void ShowAbove(PlayerControl target, string alignmentText, float duration = 2.5f)
    {
        if (target?.cosmetics?.nameTextContainer == null) return;

        // Remove any existing popup first
        var existing = target.cosmetics.nameTextContainer.transform.FindChild("AlignmentPopup");
        if (existing != null) existing.gameObject.Destroy();

        var go = new GameObject("AlignmentPopup");
        go.transform.SetParent(target.cosmetics.nameTextContainer.transform, false);
        go.transform.localPosition = new Vector3(0f, 0.35f, -1f);

        var tmp = go.AddComponent<TextMeshPro>();
        tmp.text = alignmentText;
        tmp.fontSize = 2f;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.richText = true;

        Coroutines.Start(FadeOut(go, duration));
    }

    private static System.Collections.IEnumerator FadeOut(GameObject go, float duration)
    {
        if (go == null) yield break;

        var tmp = go.GetComponent<TextMeshPro>();
        float elapsed = 0f;
        float fadeStart = duration * 0.6f; // start fading at 60% of duration

        while (elapsed < duration)
        {
            if (go == null) yield break;
            elapsed += UnityEngine.Time.deltaTime;

            // Float upward slightly
            go.transform.localPosition += new Vector3(0f, 0.003f, 0f);

            // Fade out in the last 40%
            if (elapsed > fadeStart && tmp != null)
            {
                float alpha = 1f - ((elapsed - fadeStart) / (duration - fadeStart));
                var c = tmp.color;
                c.a = alpha;
                tmp.color = c;
            }

            yield return null;
        }

        if (go != null) go.Destroy();
    }
}