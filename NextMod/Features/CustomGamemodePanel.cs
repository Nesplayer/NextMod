using HarmonyLib;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using NEXT.Features;
using NEXT.Options;        // for GeneralOptions and LaunchpadGamemode
using MiraAPI.GameOptions;

[HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.ChangeTab))]
public static class KeepGameModeButtonPatch
{
    public static void Postfix(GameSettingMenu __instance)
    {
        var leftPanel = __instance.transform.Find("LeftPanel");
        if (leftPanel == null) return;

        var customOneButton = leftPanel.Find("CustomOneButton");
        if (customOneButton == null) return;

        // Keep it visible
        customOneButton.gameObject.SetActive(true);

        // Re-apply the label
        var allTexts = customOneButton.GetComponentsInChildren<TextMeshPro>(true);
        if (allTexts.Length > 0)
        {
            foreach (var t in allTexts)
                t.text = "Game Modes";
        }
        else
        {
            var allTextsUGUI = customOneButton.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var t in allTextsUGUI)
                t.text = "Game Modes";
        }
    }
}

[HarmonyPatch(typeof(GameSettingMenu), nameof(GameSettingMenu.Start))]
public static class PlayerOptionsCubePatch {
    public static void Postfix(GameSettingMenu __instance) {
        // ── LeftPanel ──────────────────────────────────────────────────
        var leftPanel = __instance.transform.Find("LeftPanel");
        if (leftPanel == null) {
            Debug.LogError("[Launchpad] Could not find LeftPanel inside GameSettingMenu");
            return;
        }
        leftPanel.localPosition = new Vector3(0f, -0.2214f, 0f);

        // ── CustomOneButton ────────────────────────────────────────────
        var customOneButton = leftPanel.Find("CustomOneButton");
        if (customOneButton == null) {
            Debug.LogError("[Launchpad] Could not find CustomOneButton inside LeftPanel");
            return;
        }

        customOneButton.gameObject.SetActive(true);

        // Rename label
        var allTexts = customOneButton.GetComponentsInChildren < TextMeshPro > (true);
        if (allTexts.Length > 0) {
            foreach(var t in allTexts)
            t.text = "Game Modes";
        } else {
            var allTextsUGUI = customOneButton.GetComponentsInChildren < TextMeshProUGUI > (true);
            foreach(var t in allTextsUGUI)
            t.text = "Game Modes";
        }

        // ── Find MainArea and all tabs ─────────────────────────────────
        var mainArea = __instance.transform.Find("MainArea");
        if (mainArea == null) {
            Debug.LogError("[Launchpad] Could not find MainArea inside GameSettingMenu");
            return;
        }

        var presetsTabs = mainArea.Find("PRESETS TABS");
        var gameSettingsTab = mainArea.Find("GAME SETTINGS TAB");
        var rolesTab = mainArea.Find("ROLES TAB");
        var modifiersTab = mainArea.Find("MODIFIERS TAB");
        var customTab1 = mainArea.Find("CUSTOM TAB 1");
        var customTab2 = mainArea.Find("CUSTOM TAB 2");

        if (customTab1 == null) {
            Debug.LogError("[Launchpad] Could not find CUSTOM TAB 1 inside MainArea");
            return;
        }

        // Hide by default, do NOT populate yet
        customTab1.gameObject.SetActive(false);

        // ── Wire up the PassiveButton ──────────────────────────────────
        var btn = customOneButton.GetComponent < PassiveButton > ();
        if (btn == null) {
            Debug.LogError("[Launchpad] CustomOneButton has no PassiveButton component!");
            return;
        }

        bool populated = false;

        btn.OnClick = new Button.ButtonClickedEvent();
        btn.OnClick.AddListener((System.Action)(() => {
                Debug.Log("[Launchpad] Game Modes button clicked!");

                // Populate only once, lazily on first click
                if (!populated) {
                    PopulateGamemodePanel(__instance, customTab1);
                    populated = true;
                }

                if (presetsTabs != null)
                    presetsTabs.gameObject.SetActive(false);
                if (gameSettingsTab != null)
                    gameSettingsTab.gameObject.SetActive(false);
                if (rolesTab != null)
                    rolesTab.gameObject.SetActive(false);
                if (modifiersTab != null)
                    modifiersTab.gameObject.SetActive(false);
                if (customTab2 != null)
                    customTab2.gameObject.SetActive(false);

                customTab1.gameObject.SetActive(true);

                for (int i = 0; i < leftPanel.childCount; i++)
{
    var child = leftPanel.GetChild(i);
    if (child == null || child == customOneButton) continue;

    var otherBtn = child.GetComponent<PassiveButton>();
    if (otherBtn == null) continue;

    otherBtn.OnClick.AddListener((System.Action)(() =>
    {
        customOneButton.gameObject.SetActive(true);
        customTab1.gameObject.SetActive(false); // hide game modes panel when switching away
    }));
}

                btn.SelectButton(true);
            }));

        Debug.Log("[Launchpad] CustomOneButton enabled as Game Modes button!");
    }

    private static void PopulateGamemodePanel(GameSettingMenu __instance, Transform customTab1) {
        var original = __instance.PresetsTab.StandardPresetButton.gameObject;

        // Pre-load sprites once
        Sprite stopNGoSprite = LaunchpadAssets.StopNGo.LoadAsset();
        Sprite outbreakSprite = LaunchpadAssets.Outbreak.LoadAsset();
        Sprite comingSoonSprite = LaunchpadAssets.ComingSoon.LoadAsset();

        var cards = new[]{
            ("StopnGo", "Stop n Go", new Vector3(2.2f, -1.2f, 0f), false, stopNGoSprite),
            ("Outbreak", "Outbreak", new Vector3(3.8f, -1.2f, 0f), false, outbreakSprite),
            ("ComingSoon1", "Coming Soon...", new Vector3(5.4f, -1.2f, 0f), true, comingSoonSprite),
            ("ComingSoon2", "Coming Soon...", new Vector3(7f, -1.2f, 0f), true, comingSoonSprite),
            ("ComingSoon3", "Coming Soon...", new Vector3(2.2f, -3.2f, 0f), true, comingSoonSprite),
            ("ComingSoon4", "Coming Soon...", new Vector3(3.8f, -3.2f, 0f), true, comingSoonSprite),
            ("ComingSoon5", "Coming Soon...", new Vector3(5.4f, -3.2f, 0f), true, comingSoonSprite),
            ("ComingSoon6", "Coming Soon...", new Vector3(7f, -3.2f, 0f), true, comingSoonSprite),
        };

        Transform? currentlySelected = null;

    // Helper to sync visual selection across all cards
    void RefreshSelection()
    {
        for (int i = 0; i < customTab1.childCount; i++)
        {
            var sibling = customTab1.GetChild(i);
            if (sibling == null) continue;
            var sibBtn = sibling.GetComponent<PassiveButton>();
            if (sibBtn != null)
                sibBtn.SelectButton(sibling == currentlySelected);
        }
    }

    // On open, reflect whatever gamemode is already active
    var currentGamemode = OptionGroupSingleton<GeneralOptions>.Instance.Gamemode.Value;

    foreach (var (id, label, pos, comingSoon, sprite) in cards)
    {
        var card = Object.Instantiate(original, customTab1);
        card.name = $"GamemodeCard_{id}";
        card.SetActive(true);

        card.transform.localScale = new Vector3(
            original.transform.localScale.x / 2f,
            original.transform.localScale.y / 2f,
            original.transform.localScale.z);
        card.transform.localPosition = pos;

        // ── Apply sprite to all three states ───────────────────────
        var passiveBtn = card.GetComponent<PassiveButton>();
        if (passiveBtn != null && sprite != null)
        {
            if (passiveBtn.inactiveSprites != null)
            {
                var bg = passiveBtn.inactiveSprites.transform.Find("Background");
                if (bg != null) { var sr = bg.GetComponent<SpriteRenderer>(); if (sr != null) { sr.sprite = sprite; sr.size = new Vector2(4.48f, 5.23f); } }
            }
            if (passiveBtn.activeSprites != null)
            {
                var bg = passiveBtn.activeSprites.transform.Find("Background");
                if (bg != null) { var sr = bg.GetComponent<SpriteRenderer>(); if (sr != null) { sr.sprite = sprite; sr.size = new Vector2(4.48f, 5.23f); } }
            }
            if (passiveBtn.selectedSprites != null)
            {
                var bg = passiveBtn.selectedSprites.transform.Find("Background");
                if (bg != null) { var sr = bg.GetComponent<SpriteRenderer>(); if (sr != null) { sr.sprite = sprite; sr.size = new Vector2(4.48f, 5.23f); } }
            }
			if (passiveBtn.selectedInactiveSprites != null)
            {
                var bg = passiveBtn.selectedInactiveSprites.transform.Find("Background");
                if (bg != null) { var sr = bg.GetComponent<SpriteRenderer>(); if (sr != null) { sr.sprite = sprite; sr.size = new Vector2(4.48f, 5.23f); } }
            }

            // Clear old listeners
            passiveBtn.OnClick = new Button.ButtonClickedEvent();

            if (!comingSoon)
            {
                string modeName = label;
                Transform cardTransform = card.transform;

                // Pre-select if this gamemode is already active
                if ((modeName == "Stop n Go"  && currentGamemode == (int)LaunchpadGamemode.StopNGo) ||
                    (modeName == "Outbreak"   && currentGamemode == (int)LaunchpadGamemode.Outbreak))
                {
                    currentlySelected = cardTransform;
                }

                passiveBtn.OnClick.AddListener((System.Action)(() =>
                {
                    // If this card is already selected, deselect it and reset to Normal
                    if (currentlySelected == cardTransform)
                    {
                        currentlySelected = null;
                        OptionGroupSingleton<GeneralOptions>.Instance.Gamemode.SetValue(
                            (int)LaunchpadGamemode.Normal);
                        Debug.Log("[Launchpad] Gamemode reset to Normal");
                    }
                    else
                    {
                        // Select this card, deselect the old one
                        currentlySelected = cardTransform;

                        if (modeName == "Stop n Go")
                        {
                            OptionGroupSingleton<GeneralOptions>.Instance.Gamemode.SetValue(
                                (int)LaunchpadGamemode.StopNGo);
                            Debug.Log("[Launchpad] Gamemode set to Stop n Go");
                        }
                        else if (modeName == "Outbreak")
                        {
                            OptionGroupSingleton<GeneralOptions>.Instance.Gamemode.SetValue(
                                (int)LaunchpadGamemode.Outbreak);
                            Debug.Log("[Launchpad] Gamemode set to Outbreak");
                        }
                    }

                    // Refresh visuals for all cards
                    RefreshSelection();
                }));
            }
        }
        else if (sprite == null)
        {
            Debug.LogWarning($"[Launchpad] Sprite is null for card: {id}");
        }
        else if (passiveBtn == null)
        {
            Debug.LogWarning($"[Launchpad] No PassiveButton found on card: {id}");
        }

            // ── Set label ──────────────────────────────────────────────
var modeText = card.transform.Find("ModeText");
if (modeText != null)
{
    var tmp = modeText.GetComponent<TextMeshPro>();
    if (tmp != null)
    {
        tmp.text = label;

        if (comingSoon)
        {
            tmp.color = new Color(0.5f, 0.5f, 0.5f); // grey
        }
        else if (label == "Stop n Go")
        {
            tmp.color = new Color32(255, 80, 80, 255); // red — matches StopNGoOptions.GroupColor
        }
        else if (label == "Outbreak")
        {
            tmp.color = new Color32(50, 205, 50, 255); // green — matches OutbreakOptions.GroupColor
        }
        else
        {
            tmp.color = Color.white; // fallback
        }
    }
}

            Debug.Log($"[Launchpad] Created gamemode card: {label}");
        }
		RefreshSelection();

        Debug.Log("[Launchpad] Gamemode panel populated!");
    }
}
