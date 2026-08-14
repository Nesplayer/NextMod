using NEXT.Features.Managers;
using NEXT.Networking.Color;
using NEXT.Utilities;
using NEXT.Features;
using MiraAPI.GameOptions;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.GameOptions.OptionTypes;
using Reactor.Networking.Rpc;
using System.Collections.Generic;
using UnityEngine;

namespace NEXT.Options;

public class FunOptions : AbstractOptionGroup
{
    public override string GroupName => "Fun Options";
    public override Color GroupColor => LaunchpadPalette.FunMenu;

    [ModdedToggleOption("Friendly Fire")]
    public bool FriendlyFire { get; set; } = false;

    [ModdedToggleOption("Colorful Chat Bubbles")]
    public bool ColorfulBubbles { get; set; } = false;

    public ModdedToggleOption UniqueColors { get; } = new("Unique Colors", true)
    {
        ChangedEvent = value =>
        {
            if (!AmongUsClient.Instance.AmHost || !value)
            {
                return;
            }

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (GradientManager.TryGetColor(player.PlayerId, out var grad) && !player.AmOwner)
                {
                    Rpc<CustomCmdCheckColor>.Instance.Handle(player, new CustomColorData((byte)player.Data.DefaultOutfit.ColorId, grad));
                }
            }
        }
    };

    public ModdedEnumOption Character { get; } = new("Character", 0, typeof(BodyTypes))
    {
        ChangedEvent = value =>
        {
            foreach (var plr in PlayerControl.AllPlayerControls)
            {
                plr.SetBodyType((int)IntToBodyTypes[value]);
            }
        }
    };

    public static readonly Dictionary<int, PlayerBodyTypes> IntToBodyTypes = new()
    {
        [0] = PlayerBodyTypes.Normal,
        [1] = PlayerBodyTypes.Horse,
        [2] = PlayerBodyTypes.Long
    };

    public enum BodyTypes
    {
        Normal,
        Horse,
        Long,
    }
}