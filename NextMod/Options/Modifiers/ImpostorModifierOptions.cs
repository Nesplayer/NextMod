using MiraAPI.GameOptions;
using NEXT.Features;
using MiraAPI.GameOptions.Attributes;
using MiraAPI.Utilities;
using UnityEngine;

namespace NEXT.Options.Modifiers;

public class ImpostorModifierOptions : AbstractOptionGroup
{
    public override string GroupName => "Impostor Modifiers";
    public override bool ShowInModifiersMenu => true;
    public override Color GroupColor => LaunchpadPalette.Impostor;
}