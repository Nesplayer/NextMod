using MiraAPI.Colors;
using MiraAPI.Utilities;
using UnityEngine;

namespace NEXT.Features.Colors;

[RegisterCustomColors]
public static class LaunchpadColors
{
    public static CustomColor SkyBlue { get; } = new CustomColor("Sky Blue", new Color32(135, 206, 235, 255), new Color32(70, 130, 180, 255));
    public static CustomColor Salmon { get; } = new CustomColor("Salmon", new Color32(250, 128, 114, 255), new Color32(233, 150, 122, 255));
    public static CustomColor Teal { get; } = new CustomColor("Teal", new Color32(0, 128, 128, 255), new Color32(0, 100, 100, 255));
    public static CustomColor Amber { get; } = new CustomColor("Amber", new Color32(255, 191, 0, 255), new Color32(255, 165, 0, 255));
    public static CustomColor Turquoise { get; } = new CustomColor("Turquoise", new Color32(64, 224, 208, 255), new Color32(72, 209, 204, 255));
    public static CustomColor SlateGray { get; } = new CustomColor("Slate Gray", new Color32(112, 128, 144, 255), new Color32(47, 79, 79, 255));
    public static CustomColor Periwinkle { get; } = new CustomColor("Periwinkle", new Color32(204, 204, 255, 255), new Color32(196, 196, 255, 255));
    public static CustomColor LimeGreen { get; } = new CustomColor("Lime Green", new Color32(50, 205, 50, 255), new Color32(34, 139, 34, 255));
    public static CustomColor Indigo { get; } = new CustomColor("Indigo", new Color32(75, 0, 130, 255), new Color32(54, 0, 102, 255));
    public static CustomColor Apricot { get; } = new CustomColor("Apricot", new Color32(251, 206, 177, 255), new Color32(255, 160, 122, 255));
    public static CustomColor Charcoal { get; } = new CustomColor("Charcoal", new Color32(54, 69, 79, 255), new Color32(70, 70, 70, 255));
    public static CustomColor Burgundy { get; } = new CustomColor("Burgundy", new Color32(128, 0, 32, 255), new Color32(100, 0, 20, 255));
    public static CustomColor Mustard { get; } = new CustomColor("Mustard", new Color32(255, 219, 88, 255), new Color32(255, 215, 0, 255));
    public static CustomColor Emerald { get; } = new CustomColor("Emerald", new Color32(80, 200, 120, 255), new Color32(0, 201, 87, 255));
    public static CustomColor Fuchsia { get; } = new CustomColor("Fuchsia", new Color32(255, 119, 255, 255), new Color32(255, 0, 255, 255));
    public static CustomColor NavyBlue { get; } = new CustomColor("Navy Blue", new Color32(0, 0, 128, 255), new Color32(0, 0, 102, 255));
    public static CustomColor RoyalBlue { get; } = new CustomColor("Royal Blue", new Color32(65, 105, 225, 255));
    public static CustomColor PureBlack { get; } = new("Pure Black", Color.black, Color.black);
    public static CustomColor PureWhite { get; } = new("Pure White", Color.white, Color.white.DarkenColor(.05f));
    public static CustomColor HotPink { get; } = new("Hot Pink", new Color32(238, 0, 108, 255));
    public static CustomColor Blueberry { get; } = new("Blueberry", new Color32(85, 151, 207, 255));
    public static CustomColor Mint { get; } = new("Mint", new Color32(91, 190, 140, 255));
    public static CustomColor Lavender { get; } = new("Lavender", new Color32(181, 176, 255, 255));
    public static CustomColor Iris { get; } = new("Iris", new Color32(90, 79, 207, 255));
    public static CustomColor Viridian { get; } = new("Viridian", new Color32(64, 130, 109, 255));
    public static CustomColor Blurple { get; } = new("Blurple", new Color32(114, 137, 218, 255), new Color32(80, 96, 153, 255));
    public static CustomColor Coral { get; } = new("Coral", new Color32(255, 127, 80, 255), new Color32(233, 116, 81, 255));
    public static CustomColor Olive { get; } = new("Olive", new Color32(128, 128, 0, 255), new Color32(105, 105, 0, 255));
    public static CustomColor Cerulean { get; } = new("Cerulean", new Color32(42, 82, 190, 255), new Color32(30, 60, 140, 255));
    public static CustomColor Peach { get; } = new("Peach", new Color32(255, 218, 185, 255), new Color32(255, 180, 140, 255));
    public static CustomColor Magenta { get; } = new("Magenta", new Color32(255, 0, 255, 255), new Color32(200, 0, 200, 255));
    public static CustomColor Crimson { get; } = new("Crimson", new Color32(220, 20, 60, 255), new Color32(180, 0, 40, 255));
    public static CustomColor Aqua { get; } = new("Aqua", new Color32(0, 255, 255, 255), new Color32(0, 200, 200, 255));
    public static CustomColor Tangerine { get; } = new("Tangerine", new Color32(242, 133, 0, 255), new Color32(200, 100, 0, 255));
    public static CustomColor SlateBlue { get; } = new("Slate Blue", new Color32(106, 90, 205, 255), new Color32(80, 70, 160, 255));
    public static CustomColor Lime { get; } = new("Lime", new Color32(0, 255, 0, 255), new Color32(0, 200, 0, 255));
    public static CustomColor Chocolate { get; } = new("Chocolate", new Color32(210, 105, 30, 255), new Color32(160, 80, 20, 255));
    public static CustomColor Orchid { get; } = new("Orchid", new Color32(218, 112, 214, 255), new Color32(180, 80, 180, 255));
    public static CustomColor TealGreen { get; } = new("Teal Green", new Color32(0, 130, 120, 255), new Color32(0, 100, 90, 255));
    public static CustomColor Ruby { get; } = new("Ruby", new Color32(224, 17, 95, 255), new Color32(180, 10, 80, 255));
    public static CustomColor SkyMagenta { get; } = new("Sky Magenta", new Color32(207, 113, 207, 255), new Color32(170, 80, 170, 255));
    public static CustomColor MintGreen { get; } = new("Mint Green", new Color32(152, 255, 152, 255), new Color32(100, 200, 100, 255));
    public static CustomColor MidnightBlue { get; } = new("Midnight Blue", new Color32(25, 25, 112, 255), new Color32(10, 10, 80, 255));
    public static CustomColor AmberRose { get; } = new("Amber Rose", new Color32(255, 120, 150, 255), new Color32(220, 80, 120, 255));
    public static CustomColor Cerise { get; } = new("Cerise", new Color32(222, 49, 99, 255), new Color32(180, 30, 80, 255));
    public static CustomColor PaleGold { get; } = new("Pale Gold", new Color32(238, 232, 170, 255), new Color32(200, 180, 120, 255));
}