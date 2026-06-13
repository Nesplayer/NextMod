using MiraAPI.GameOptions;
using TORWL.Features;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using UnityEngine;

namespace TORWL.Options.Modifiers;

public class ImpostorModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "Impostor Modifiers";
    public override bool ShowInModifiersMenu => true;
    public override Color GroupColor => LaunchpadPalette.Impostor;
}