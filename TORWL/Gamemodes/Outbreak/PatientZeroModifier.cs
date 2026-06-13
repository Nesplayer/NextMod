using MiraAPI.Modifiers;
using UnityEngine;

namespace TORWL.Features.Outbreak;

/// <summary>
/// Applied to exactly one player at game start — the Patient 0.
/// They are assigned the Impostor role so they have a kill button;
/// their kills are intercepted by OutbreakMode to spread infection instead.
/// </summary>
public class PatientZeroModifier : BaseModifier
{
    public override string ModifierName => "PatientZero";

    // ── Singleton guard ───────────────────────────────────────────────────────

    /// <summary>True while a Patient 0 modifier is active in the current game.</summary>
    public static bool IsAssigned { get; internal set; }

    private static readonly int VisorColorId = Shader.PropertyToID("_VisorColor");

    private static readonly Color PatientZeroVisorColor = new Color32(220, 30, 30, 255);

    private Color _originalVisorColor;
    private string _originalName = string.Empty;

    private const string NameSuffix = " <color=#DC1E1E>☣ Patient 0</color>";

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    public override void OnActivate()
    {
        // Hard guard: only one Patient 0 may exist at a time.
        if (IsAssigned)
        {
            // Remove ourselves immediately so the modifier doesn't stick.
            Player?.GetComponent<ModifierComponent>()?.RemoveModifier(this);
            return;
        }

        IsAssigned = true;

        if (Player == null) return;

        // Strip hat / skin cosmetics to emphasise the role
        Player.cosmetics.hat.gameObject.SetActive(false);
        Player.cosmetics.skin.gameObject.SetActive(false);

        // Save & override visor colour
        var mat = Player.cosmetics.currentBodySprite.BodySprite.material;
        _originalVisorColor = mat.GetColor(VisorColorId);
        mat.SetColor(VisorColorId, PatientZeroVisorColor);

        // Append name tag
        if (Player.cosmetics.nameText != null)
        {
            _originalName = Player.cosmetics.nameText.text;
            Player.cosmetics.nameText.text = _originalName + NameSuffix;
        }
    }

    public override void OnDeactivate()
    {
        IsAssigned = false;

        if (Player == null) return;

        // Restore visor
        Player.cosmetics.currentBodySprite.BodySprite.material
            .SetColor(VisorColorId, _originalVisorColor);

        // Restore cosmetics visibility
        Player.cosmetics.hat.gameObject.SetActive(true);
        Player.cosmetics.skin.gameObject.SetActive(true);

        // Restore name
        if (Player.cosmetics.nameText != null)
            Player.cosmetics.nameText.text = _originalName;
    }

    public override void FixedUpdate()
    {
        // Keep visor colour correct during meetings
        if (MeetingHud.Instance == null) return;
        foreach (var state in MeetingHud.Instance.playerStates)
        {
            if (state.TargetPlayerId != Player!.PlayerId) continue;
            state.PlayerIcon.cosmetics.currentBodySprite.BodySprite.material
                .SetColor(VisorColorId, PatientZeroVisorColor);
            break;
        }
    }
}