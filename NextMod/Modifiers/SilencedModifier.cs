using MiraAPI.Modifiers;
using MiraAPI.Utilities.Assets;
using NEXT.Components;
using NEXT.Features;
using NEXT.Utilities;
using UnityEngine;
using MiraAPI.Utilities;

namespace NEXT.Modifiers;

public class SilencedModifier : BaseModifier
{
    public override string ModifierName => "Silenced";
    public override LoadableAsset<Sprite>? ModifierIcon => LaunchpadAssets.Silenced;
    public override string GetDescription() => $"You have been <b>silenced</b>! This means you're unable to send messages in chat until the meeting ends.";
    private bool meetingWasActive = false;

    private readonly PlayerTag _silencedTag = new()
    {
        Name = "SilencedTag",
        Text = "Silenced",
        Color = LaunchpadPalette.SilencerColor,
        IsLocallyVisible = _ => true,
    };

    public override void OnActivate()
    {
        var tagManager = Player!.GetTagManager();

        if (tagManager != null)
        {
            var existingTag = tagManager.GetTagByName(_silencedTag.Name);
            if (existingTag.HasValue)
            {
                tagManager.RemoveTag(existingTag.Value);
            }

            tagManager.AddTag(_silencedTag);
        }

        NotifyOfSilenced();
    }

    public override void OnDeactivate()
    {
        var tagManager = Player?.GetTagManager();

        if (tagManager != null)
        {
            tagManager.RemoveTag(_silencedTag);
        }
    }

    public override void OnDeath(DeathReason reason)
    {
        ModifierComponent!.RemoveModifier(this);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        var meetingInstance = MeetingHud.Instance;

        if (meetingInstance != null)
        {
            meetingWasActive = true;
        }
        else
        {
            if (meetingWasActive)
            {
                meetingWasActive = false;
                ModifierComponent!.RemoveModifier(this);
            }
            return;
        }
    }

    public void NotifyOfSilenced()
    {
        MiraAPI.Utilities.Helpers.CreateAndShowNotification(
            $"{Player.Data.PlayerName} has been silenced!",
            LaunchpadPalette.SilencedColor,
            null,
            LaunchpadAssets.Silenced.LoadAsset()
        ).transform.localPosition = new Vector3(0f, 1f, -20f);
    }
}