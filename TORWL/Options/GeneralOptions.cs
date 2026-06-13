using MiraAPI.GameModes;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using System;
using TORWL.Features;
using UnityEngine;
using MiraAPI.Utilities;

namespace TORWL.Options;

public class GeneralOptions : AbstractOptionGroup
{
    public override string GroupName => "General";
    public override Color GroupColor => LaunchpadPalette.GeneralMenu;
    public override Func<bool> GroupVisible => CustomGameModeManager.IsDefault;

    public ModdedEnumOption Gamemode { get; } =
    new(
        "Gamemode",
        0,
        typeof(LaunchpadGamemode),
        new[]
        {
            "Normal",
            "<color=#ff5050>Stop 'n Go</color>",
            "<color=#32cd32>Outbreak</color>"
        }
    );

    public ModdedToggleOption Notepad { get; set; } = new("Notepad", true)
    {
        ChangedEvent = value =>
        {
            NotepadHud.Instance?.SetNotepadButtonVisible(value);
        }
    };

    [ModdedToggleOption("Ban Cheaters")] public bool BanCheaters { get; set; } = true;
    [ModdedToggleOption("Disable Meeting Teleport")] public bool DisableMeetingTeleport { get; set; } = false;
    [ModdedToggleOption("Auto-Start Lobby")] public bool AutoStart { get; set; } = false;

    [ModdedNumberOption("Auto-Start After", 10f, 400f, 10f, MiraNumberSuffixes.Seconds)]
    public float AutoStartAfter { get; set; } = 100f;

    [ModdedNumberOption("Minimum Players", 1f, 15f, 1f)]
    public float AutoStartMinPlayers { get; set; } = 5f;
}

public enum LaunchpadGamemode
{
    Normal,
    StopNGo,
    Outbreak,
}