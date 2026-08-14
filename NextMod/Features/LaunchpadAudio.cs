using MiraAPI.Utilities.Assets;
using UnityEngine;

namespace NEXT.Features;

public static class LaunchpadAudio
{
    // THIS FILE SHOULD ONLY HOLD AUDIO
    public static LoadableAudioResourceAsset MagicWhoosh { get; } = new LoadableAudioResourceAsset("NEXTaunchpad.Resources.Sounds.Whoosh.wav");
    public static LoadableAudioResourceAsset Potion { get; } = new LoadableAudioResourceAsset("NEXTaunchpad.Resources.Sounds.Potion.wav");
    public static LoadableAudioResourceAsset Curse { get; } = new LoadableAudioResourceAsset("NEXTaunchpad.Resources.Sounds.Curse.wav");
}