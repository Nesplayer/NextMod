using MiraAPI.Modifiers;
using UnityEngine;

namespace TORWL.Features.Outbreak;

/// <summary>
/// Applied to any player infected after game start (not Patient 0).
/// Secondary infected get a green visor and a name suffix to distinguish
/// them visually from Patient 0, but they spread infection identically.
/// </summary>
public class InfectedModifier : BaseModifier
{
    public override string ModifierName => "Infected";

    private static readonly int VisorColorId = Shader.PropertyToID("_VisorColor");

    // Sickly green — distinct from Patient 0's red
    private static readonly Color InfectedVisorColor = new Color32(50, 205, 50, 255);

    private Color _originalVisorColor;
    private string _originalName = string.Empty;

    private const string NameSuffix = " <color=#32CD32>☣ Infected</color>";

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    public override void OnActivate()
    {
        if (Player == null) return;

        // Strip hat / skin cosmetics (pet removed — not in CosmeticsLayer API)
        Player.cosmetics.hat.gameObject.SetActive(false);
        Player.cosmetics.skin.gameObject.SetActive(false);

        // Save & override visor colour
        var mat = Player.cosmetics.currentBodySprite.BodySprite.material;
        _originalVisorColor = mat.GetColor(VisorColorId);
        mat.SetColor(VisorColorId, InfectedVisorColor);

        // Append name tag via nameText (replaces GetTagManager which is not in vanilla AU)
        if (Player.cosmetics.nameText != null)
        {
            _originalName = Player.cosmetics.nameText.text;
            Player.cosmetics.nameText.text = _originalName + NameSuffix;
        }
    }

    public override void OnDeactivate()
    {
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
                .SetColor(VisorColorId, InfectedVisorColor);
            break;
        }
    }
}