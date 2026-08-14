using AmongUs.GameOptions;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using HarmonyLib;
using MiraAPI.GameOptions;
using MiraAPI.Modifiers;
using TMPro;
using NEXT.Options;
using UnityEngine;

namespace NEXT.Features.Outbreak;

[HarmonyPatch]
public static class OutbreakPatches
{
    private static bool IsOutbreakEnabled =>
        OptionGroupSingleton<GeneralOptions>.Instance.Gamemode.Value == (int)LaunchpadGamemode.Outbreak;

    // ── Game start ────────────────────────────────────────────────────────────

    [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.StartGame))]
    [HarmonyPostfix]
    public static void StartGame_Postfix()
    {
        if (!AmongUsClient.Instance.AmHost) return;
        if (!IsOutbreakEnabled) return;
        OutbreakMode.Activate();
    }

    // ── Assign roles: everyone Crewmate except one random Impostor (Patient 0) ─

    [HarmonyPatch(typeof(RoleManager), nameof(RoleManager.SelectRoles))]
    [HarmonyPrefix]
    public static bool SelectRoles_Prefix()
    {
        if (!OutbreakMode.IsActive) return true;

        // Pick Patient 0 before assigning roles
        var players = PlayerControl.AllPlayerControls;
        if (players.Count == 0) return false;

        var patient0 = players[Random.Range(0, players.Count)];
        OutbreakMode.Patient0Id = patient0.PlayerId;

        foreach (var pc in PlayerControl.AllPlayerControls)
        {
            if (pc == null) continue;
            // Patient 0 gets Impostor so they have a kill button and are
            // recognised as a threat by the game's own win-condition logic.
            if (pc.PlayerId == OutbreakMode.Patient0Id)
                pc.RpcSetRole(RoleTypes.Impostor, true);
            else
                pc.RpcSetRole(RoleTypes.Crewmate, true);
        }
        return false;
    }

    // ── Attach PatientZeroModifier after roles are locked in ─────────────────

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.StartGame))]
    [HarmonyPostfix]
    public static void GameManagerStart_Postfix()
    {
        if (!AmongUsClient.Instance.AmHost) return;
        if (!OutbreakMode.IsActive) return;
        if (OutbreakMode.Patient0Id < 0) return;

        PlayerControl? patient0 = null;
        foreach (var pc in PlayerControl.AllPlayerControls)
        {
            if (pc != null && pc.PlayerId == OutbreakMode.Patient0Id) { patient0 = pc; break; }
        }
        if (patient0 == null) return;

        var modComp = patient0.GetComponent<ModifierComponent>();
        modComp?.AddModifier<PatientZeroModifier>();
    }

    // ── Custom intro screen ───────────────────────────────────────────────────

    // AU drives the role card through IntroCutscene.
    // BeginImpostor fires for Patient 0; BeginCrewmate fires for everyone else.

    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginImpostor))]
    [HarmonyPostfix]
    public static void BeginImpostor_Postfix(IntroCutscene __instance)
    {
        if (!OutbreakMode.IsActive) return;
        SetIntroText(__instance, "Patient 0", new Color32(220, 30, 30, 255), "Infect them all to win");
    }

    [HarmonyPatch(typeof(IntroCutscene), nameof(IntroCutscene.BeginCrewmate))]
    [HarmonyPostfix]
    public static void BeginCrewmate_Postfix(IntroCutscene __instance)
    {
        if (!OutbreakMode.IsActive) return;
        SetIntroText(__instance, "Crewmate", new Color32(140, 255, 140, 255), "There is 1 Patient 0 among you, survive!");
    }

    private static void SetIntroText(IntroCutscene instance, string title, Color titleColor, string blurb)
    {
        // Field names confirmed from Unity hierarchy: RoleText_TMP, RoleBlurb, YouAreText
        var roleText = instance.transform.FindChild("Role Intro/RoleText_TMP")?.GetComponent<TextMeshPro>();
        var roleBlurb = instance.transform.FindChild("Role Intro/RoleBlurb")?.GetComponent<TextMeshPro>();
        var youAreText = instance.transform.FindChild("Role Intro/YouAreText")?.GetComponent<TextMeshPro>();

        if (roleText != null)
        {
            roleText.text = title;
            roleText.color = titleColor;
        }
        if (roleBlurb != null)
        {
            roleBlurb.text = blurb;
        }
        if (youAreText != null)
        {
            // Keep vanilla "You are the" for crewmates, customise for Patient 0
            youAreText.text = title == "Patient 0" ? "You are" : "You are the";
        }
    }

    // ── Task completion → slow infection ─────────────────────────────────────

    [HarmonyPatch(typeof(PlayerTask), nameof(PlayerTask.Complete))]
    [HarmonyPostfix]
    public static void TaskComplete_Postfix()
    {
        if (!OutbreakMode.IsActive) return;
        if (!AmongUsClient.Instance.AmHost) return;
        OutbreakMode.OnTaskCompleted();
        OutbreakMode.CheckCrewWin();
    }

    // ── HUD tick — name tinting + hide kill button for non-Patient-0 ─────────

    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Update))]
    [HarmonyPostfix]
    public static void HudUpdate_Postfix(HudManager __instance)
    {
        if (!OutbreakMode.IsActive) return;
        OutbreakHud.Tick();

        // Only Patient 0 (the Impostor) should have a kill button.
        // Hide it for everyone else; Patient 0 uses it to start infections.
        bool isPatient0 = PlayerControl.LocalPlayer != null &&
                          PlayerControl.LocalPlayer.PlayerId == OutbreakMode.Patient0Id;

        if (!isPatient0 && __instance.KillButton != null)
            __instance.KillButton.gameObject.SetActive(false);
    }

    // ── Freeplay support ──────────────────────────────────────────────────────

    [HarmonyPatch(typeof(TutorialManager), nameof(TutorialManager.Awake))]
    [HarmonyPostfix]
    public static void TutorialManager_Awake_Postfix()
    {
        if (!IsOutbreakEnabled) return;

        AmongUsClient.Instance.StartCoroutine(
            FreeplayDelay().WrapToIl2Cpp()
        );
    }

    private static System.Collections.IEnumerator FreeplayDelay()
    {
        yield return new UnityEngine.WaitForSeconds(1f);
        OutbreakMode.Activate();

        var local = PlayerControl.LocalPlayer;
        if (local == null) yield break;

        // In freeplay, SelectRoles never runs so we must set the role manually.
        local.RpcSetRole(RoleTypes.Impostor, true);

        OutbreakMode.Patient0Id = local.PlayerId;
        var modComp = local.GetComponent<ModifierComponent>();
        modComp?.AddModifier<PatientZeroModifier>();
    }
}