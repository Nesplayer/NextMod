using Il2CppInterop.Runtime.Attributes;
using MiraAPI.Modifiers;
using MiraAPI.Modifiers.Types;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Attributes;
using System;
using System.Linq;
using TORWL.Features;
using UnityEngine;
using UnityEngine.Events;

namespace TORWL.Components;

[RegisterInIl2Cpp]
public sealed class SetModifierMinigame(IntPtr ptr) : Minigame(ptr)
{
    private ShapeshifterPanel _panelPrefab = null!;
    private Action<GameModifier> _onClick = null!;
    private Scroller _scroller = null!;

    private PassiveButton _closeButton = null!;
    private PassiveButton _outsideCloseButton = null!;

    public void Awake()
    {
        _outsideCloseButton = transform.FindChild("Background/OutsideCloseButton").GetComponent<PassiveButton>();
        _closeButton = transform.FindChild("CloseButton").GetComponent<PassiveButton>();

        _closeButton.OnClick.AddListener((UnityAction)(() => Close()));
        _outsideCloseButton.OnClick.AddListener((UnityAction)(() => Close()));

        _panelPrefab = transform.FindChild("Panel").gameObject.GetComponent<ShapeshifterPanel>();
        _scroller = transform.FindChild("Scroller").gameObject.GetComponent<Scroller>();

        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public static SetModifierMinigame Create()
    {
        var gameObject = Instantiate(LaunchpadAssets.RoleMinigame.LoadAsset(), HudManager.Instance.transform);
        var minigame = gameObject.AddComponent<SetModifierMinigame>();
        return minigame;
    }

    [HideFromIl2Cpp]
    private static void SetModifier(ShapeshifterPanel panel, GameModifier modifier, Action onClick)
    {
        panel.shapeshift = onClick;
        panel.Button.ClickSound = HudManager.Instance.MapButton.ClickSound;

        panel.NameText.text = "<font=\"LiberationSans SDF\" material=\"LiberationSans SDF - Chat Message Masked\">" + modifier.ModifierName + "</font>";
        panel.NameText.color = Color.white;
        panel.Background.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        panel.gameObject.SetActive(true);

        var roleIcon = panel.transform.FindChild("RoleIcon").gameObject.GetComponent<SpriteRenderer>();

        if (modifier.ModifierIcon != null)
        {
            var sprite = modifier.ModifierIcon.LoadAsset();
            if (sprite != null)
            {
                roleIcon.sprite = sprite;
            }
            else
            {
                DisableIcon(panel, roleIcon);
            }
        }
        else
        {
            DisableIcon(panel, roleIcon);
        }
    }

    private static void DisableIcon(ShapeshifterPanel panel, SpriteRenderer roleIcon)
    {
        roleIcon.gameObject.SetActive(false);
        var rectTransform = panel.NameText.gameObject.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(0, 0, -15);
        rectTransform.sizeDelta = new Vector2(2.5f, 0.3726f);
    }

    [HideFromIl2Cpp]
    public void Open(Func<GameModifier, bool> modifierMatch, Action<GameModifier?> clickHandler)
    {
        _onClick = clickHandler;

        _closeButton.OnClick.AddListener((UnityAction)(() => clickHandler(null)));
        _outsideCloseButton.OnClick.AddListener((UnityAction)(() => clickHandler(null)));

        var modifiers = ModifierManager.Modifiers
            .ToArray()
            .Select(m => m as GameModifier)
            .Where(m => m != null && modifierMatch(m!))
            .ToArray();

        foreach (var modifier in modifiers)
        {
            var shapeshifterPanel = Instantiate(_panelPrefab, _scroller.Inner);
            SetModifier(shapeshifterPanel, modifier!, () => { _onClick(modifier!); });
        }

        _scroller.SetBounds(new FloatRange(0, modifiers.Length * 0.5f - 2), new FloatRange(0, 0));
        Begin(null);
    }
}