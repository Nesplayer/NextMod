using HarmonyLib;
using MiraAPI.GameOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using NEXT.Roles;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace NEXT.Patches;

[HarmonyPatch]
public static class ModifierInfoButtonPatch
{
    private static readonly Dictionary<string, Func<string>> DescriptionCache = new();

    public static readonly List<(NumberOption option, SpriteRenderer? sr, TextMeshPro? tmp, BoxCollider2D col, Func<(float minY, float maxY)> getBounds)> CullingTargets = new();

    private const float FadeSpeed = 8f;
    private const float FadeMargin = 0.15f;

    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.ChangeTab))]
    [HarmonyPostfix]
    public static void Postfix(GameSettingMenu __instance)
    {
        CullingTargets.Clear();
        BuildDescriptionCache();
        SetupInfoButtons(__instance);
    }

    [HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Update))]
    [HarmonyPostfix]
    public static void UpdatePostfix()
    {
        foreach (var (option, sr, tmp, col, getBounds) in CullingTargets)
        {
            try
            {
                if (option == null) continue;
                var (minY, maxY) = getBounds();
                float worldY = option.transform.position.y;

                float alphaBottom = Mathf.InverseLerp(minY, minY + FadeMargin, worldY);
                float alphaTop = Mathf.InverseLerp(maxY, maxY - FadeMargin, worldY);
                float targetAlpha = Mathf.Min(alphaBottom, alphaTop);

                col.enabled = targetAlpha > 0f;

                if (sr != null)
                {
                    var c = sr.color;
                    c.a = Mathf.MoveTowards(c.a, targetAlpha, FadeSpeed * Time.deltaTime);
                    sr.color = c;
                    sr.enabled = c.a > 0.01f;
                }

                if (tmp != null)
                {
                    var c = tmp.color;
                    c.a = Mathf.MoveTowards(c.a, targetAlpha, FadeSpeed * Time.deltaTime);
                    tmp.color = c;
                    tmp.enabled = c.a > 0.01f;
                }
            }
            catch { }
        }
    }

    private static void BuildDescriptionCache()
    {
        DescriptionCache.Clear();

        var allGroupTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => { try { return a.GetTypes(); } catch { return Array.Empty<Type>(); } })
            .Where(t => !t.IsAbstract && IsModifierDescriptionGroup(t, out _))
            .ToList();

        foreach (var groupType in allGroupTypes)
        {
            if (!IsModifierDescriptionGroup(groupType, out var modifierType)) continue;

            try
            {
                var singletonType = typeof(OptionGroupSingleton<>).MakeGenericType(groupType);
                var instanceProp = singletonType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static);
                if (instanceProp?.GetValue(null) is not AbstractOptionGroup groupInstance) continue;

                var descInstance = Activator.CreateInstance(modifierType) as IModifierDescription;
                if (descInstance == null) continue;

                var capturedDesc = descInstance;
                DescriptionCache[groupInstance.GroupName] = () => capturedDesc.WikiDescription;
            }
            catch { }
        }
    }

    private static bool IsModifierDescriptionGroup(Type groupType, out Type modifierType)
    {
        modifierType = null!;
        var baseType = groupType.BaseType;
        while (baseType != null)
        {
            if (baseType.IsGenericType &&
                baseType.GetGenericTypeDefinition() == typeof(AbstractOptionGroup<>))
            {
                var arg = baseType.GetGenericArguments()[0];
                if (typeof(IModifierDescription).IsAssignableFrom(arg))
                {
                    modifierType = arg;
                    return true;
                }
            }
            baseType = baseType.BaseType;
        }
        return false;
    }

    private static void SetupInfoButtons(GameSettingMenu menu)
    {
        var whatIsThis = menu.transform.Find("What Is This?");
        if (whatIsThis == null)
            whatIsThis = menu.GetComponentsInChildren<Transform>(true)
                .FirstOrDefault(t => t.name == "What Is This?");

        if (whatIsThis == null)
        {
            Debug.LogError("[Launchpad] ModifierInfoButtonPatch: Could not find 'What Is This?'");
            return;
        }

        var infoText = whatIsThis.Find("InfoText")?.GetComponent<TextMeshPro>();
        infoText ??= whatIsThis.GetComponentInChildren<TextMeshPro>(true);
        if (infoText == null)
        {
            Debug.LogError("[Launchpad] ModifierInfoButtonPatch: Could not find InfoText TMP");
            return;
        }

        var allNumberOptions = menu.GetComponentsInChildren<NumberOption>(true);
        if (allNumberOptions.Length == 0)
        {
            Debug.LogWarning("[Launchpad] ModifierInfoButtonPatch: No NumberOptions found");
            return;
        }

        Transform? boundsRef = null;
        var knownViewportNames = new[] { "Scroller", "ScrollArea", "Mask", "Viewport", "Container", "ScrollView", "Content" };
        foreach (var option in allNumberOptions)
        {
            var t = option.transform.parent;
            while (t != null)
            {
                if (knownViewportNames.Any(n => t.name.Contains(n, StringComparison.OrdinalIgnoreCase))
                    || t.GetComponent<SpriteMask>() != null)
                {
                    boundsRef = t;
                    break;
                }
                t = t.parent;
            }
            if (boundsRef != null) break;
        }

        Func<(float minY, float maxY)> getBounds;

        if (boundsRef != null)
        {
            var maskArea = boundsRef.Find("MaskArea") ?? boundsRef.Find("MaskBg");
            var capturedBoundsRef = maskArea != null ? maskArea : boundsRef;

            getBounds = () =>
            {
                var renderer = capturedBoundsRef.GetComponent<Renderer>();
                if (renderer != null)
                    return (renderer.bounds.min.y, renderer.bounds.max.y);

                var rc = capturedBoundsRef.GetComponent<RectTransform>();
                if (rc != null)
                {
                    var corners = new Vector3[4];
                    rc.GetWorldCorners(corners);
                    return (corners[0].y, corners[1].y);
                }

                return (float.MinValue, float.MaxValue);
            };
        }
        else
        {
            getBounds = () => (float.MinValue, float.MaxValue);
        }

        foreach (var numberOption in allNumberOptions)
        {
            var minusButton = numberOption.transform.Find("MinusButton");
            if (minusButton == null) continue;
            if (numberOption.transform.Find("InfoButton") != null) continue;
            if (!numberOption.name.Contains("Chance", StringComparison.OrdinalIgnoreCase)) continue;

            var matchingDesc = FindDescriptionForOption(numberOption);
            if (matchingDesc == null) continue;

            var infoButtonGo = new GameObject("InfoButton");
            infoButtonGo.transform.SetParent(numberOption.transform, false);
            infoButtonGo.layer = numberOption.gameObject.layer;

            var minusRect = minusButton.GetComponent<RectTransform>();
            var rect = infoButtonGo.AddComponent<RectTransform>();
            rect.sizeDelta = minusRect != null ? minusRect.sizeDelta : new Vector2(0.4f, 0.4f);
            rect.localPosition = new Vector3(-3.8f, 0f, 0f);
            rect.localScale = Vector3.one;

            SpriteRenderer? capturedSr = null;

            var buttonSprite = minusButton.transform.Find("ButtonSprite");
            if (buttonSprite != null)
            {
                var sourceSr = buttonSprite.GetComponent<SpriteRenderer>();
                if (sourceSr != null)
                {
                    var spriteGo = new GameObject("ButtonSprite");
                    spriteGo.transform.SetParent(infoButtonGo.transform, false);
                    spriteGo.layer = infoButtonGo.layer;
                    var sr = spriteGo.AddComponent<SpriteRenderer>();
                    sr.sprite = sourceSr.sprite;
                    sr.color = Color.white;
                    sr.sortingLayerName = sourceSr.sortingLayerName;
                    sr.sortingOrder = sourceSr.sortingOrder + 1;
                    spriteGo.transform.localPosition = Vector3.zero;
                    spriteGo.transform.localScale = new Vector3(0.2f, 0.2f, 1f);
                    capturedSr = sr;
                }
            }

            var col = infoButtonGo.AddComponent<BoxCollider2D>();
            col.size = rect.sizeDelta;

            var passiveButton = infoButtonGo.AddComponent<PassiveButton>();
            passiveButton.OnClick = new Button.ButtonClickedEvent();
            passiveButton.OnMouseOver = new UnityEngine.Events.UnityEvent();
            passiveButton.OnMouseOut = new UnityEngine.Events.UnityEvent();

            passiveButton.OnMouseOver.AddListener((UnityEngine.Events.UnityAction)(() =>
            {
                if (capturedSr != null) capturedSr.color = Color.gray;
            }));
            passiveButton.OnMouseOut.AddListener((UnityEngine.Events.UnityAction)(() =>
            {
                if (capturedSr != null) capturedSr.color = Color.white;
            }));

            var descFunc = matchingDesc;
            var wit = whatIsThis;
            var capturedInfoText = infoText;

            passiveButton.OnClick.AddListener((UnityEngine.Events.UnityAction)(() =>
            {
                if (capturedSr != null) capturedSr.color = Color.white;
                capturedInfoText.text = descFunc();
                wit.gameObject.SetActive(true);
            }));

            var textGo = new GameObject("Text_TMP");
            textGo.transform.SetParent(infoButtonGo.transform, false);
            textGo.layer = infoButtonGo.layer;

            var tmp = textGo.AddComponent<TextMeshPro>();
            tmp.text = "?";
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.fontSize = 2.5f;
            tmp.color = Color.black;

            var mr = textGo.GetComponent<MeshRenderer>();
            if (mr != null)
            {
                mr.sortingLayerName = "UI";
                mr.sortingOrder = 20;
            }

            var textRect = textGo.GetComponent<RectTransform>();
            textRect.sizeDelta = rect.sizeDelta;
            textRect.localPosition = Vector3.zero;
            textRect.localScale = Vector3.one;

            CullingTargets.Add((numberOption, capturedSr, tmp, col, getBounds));
        }
    }

    private static Func<string>? FindDescriptionForOption(NumberOption option)
    {
        var t = option.transform.parent;
        while (t != null)
        {
            if (DescriptionCache.TryGetValue(t.name, out var desc))
                return desc;
            t = t.parent;
        }

        foreach (var kvp in DescriptionCache)
        {
            if (option.name.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase) ||
                kvp.Key.Contains(option.name, StringComparison.OrdinalIgnoreCase))
                return kvp.Value;
        }

        return null;
    }
}