using Il2CppInterop.Runtime.Attributes;
using Reactor.Utilities.Attributes;
using Reactor.Utilities.ImGui;
using NEXT.Roles.Impostor;
using NEXT.Roles.Coven;
using NEXT.Roles.Crewmate;
using NEXT.Roles.Neutral;
using NEXT.Modifiers.Game.Crewmate;
using NEXT.Modifiers.Game.Universal;
using NEXT.Features;
using UnityEngine;
using MiraAPI.Modifiers;
using MiraAPI.Roles;
using AmongUs.GameOptions;
using MiraAPI.Networking;
using Reactor.Utilities.Extensions;

namespace NEXT;

[RegisterInIl2Cpp]
public class DebugWindow(nint ptr) : MonoBehaviour(ptr)
{
    private static Vector2 scrollPos = Vector2.zero;
    public static bool Visible = false;
    private readonly KeyCode toggleKey = KeyCode.F1;

    [HideFromIl2Cpp]
    public static bool AllowDebug() =>
        AmongUsClient.Instance && AmongUsClient.Instance.NetworkMode == NetworkModes.FreePlay;

    public readonly DragWindow DebuggingWindow = new(
        new Rect(10, 10, 350, 500),
        "Debug Window",
        () =>
        {
        bool allow = AllowDebug();

        scrollPos = GUILayout.BeginScrollView(
            scrollPos,
            GUILayout.Width(330),
            GUILayout.Height(470)
        );

        // -------------------- HEADER FUNCTION --------------------
        void Header(string name)
        {
            GUILayout.Space(10);
            var bold = new GUIStyle(GUI.skin.label)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 14,
            };
            GUILayout.Label(name, bold);
            GUILayout.Space(5);
        }

        // ============================
        //        COVEN ROLES
        // ============================
        Header($"<color=#{LaunchpadPalette.Coven.ToHtmlStringRGBA()}>COVEN ROLES</color>");

        if (GUILayout.Button("Become Tavern Keeper") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<TavernKeeperRole>(), false);
        
        if (GUILayout.Button("Become Scribe") && allow)
                    PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<ScribeRole>(), false);
		
		if (GUILayout.Button("Become Hexbinder") && allow)
                    PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<HexbinderRole>(), false);
		
		if (GUILayout.Button("Become Alchemist") && allow)
                    PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<AlchemistRole>(), false);
		
		if (GUILayout.Button("Become Poisoner") && allow)
                    PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<PoisonerRole>(), false);
		
		if (GUILayout.Button("Become Dominator") && allow)
                    PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<DominatorRole>(), false);
		
		if (GUILayout.Button("Become Shroudweaver") && allow)
                    PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<ShroudweaverRole>(), false);

        // ============================
        //       CREWMATE ROLES
        // ============================
        Header($"<color=#{Palette.CrewmateBlue.ToHtmlStringRGBA()}>CREWMATE ROLES</color>");

        if (GUILayout.Button("Become Coroner") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<CoronerRole>(), false);

        if (GUILayout.Button("Become Gambler") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<GamblerRole>(), false);

        if (GUILayout.Button("Become Medic") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<MedicRole>(), false);

        if (GUILayout.Button("Become Sealer") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<SealerRole>(), false);

        if (GUILayout.Button("Become Sheriff") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<SheriffRole>(), false);

        if (GUILayout.Button("Become Teleporter") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<TeleporterRole>(), false);
        
        if (GUILayout.Button("Become Shielder") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<ShielderRole>(), false);

        // ============================
        //        IMPOSTOR ROLES
        // ============================
        Header($"<color=#{Palette.ImpostorRed.ToHtmlStringRGBA()}>IMPOSTOR ROLES</color>");

        if (GUILayout.Button("Become Burrower") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<BurrowerRole>(), false);

        if (GUILayout.Button("Become Hitman") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<HitmanRole>(), false);

        if (GUILayout.Button("Become Janitor") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<JanitorRole>(), false);

        if (GUILayout.Button("Become Silencer") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<SilencerRole>(), false);

        if (GUILayout.Button("Become Surgeon") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<SurgeonRole>(), false);

        if (GUILayout.Button("Become Swapshifter") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<SwapshifterRole>(), false);

        if (GUILayout.Button("Become Poltergeist") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<PoltergeistRole>(), false);

        // ============================
        //        NUETRAL ROLES
        // ============================
        Header($"<color=#{LaunchpadPalette.Neutral.ToHtmlStringRGBA()}>NEUTRAL ROLES</color>");

        if (GUILayout.Button("Become Executioner") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<ExecutionerRole>(), false);

        if (GUILayout.Button("Become Jester") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<JesterRole>(), false);

        if (GUILayout.Button("Become Neutral Killer") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<NeutralKillerRole>(), false);

        if (GUILayout.Button("Become Reaper") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<ReaperRole>(), false);

        if (GUILayout.Button("Become Traitor") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<TraitorRole>(), false);
        
        if (GUILayout.Button("Become Survivor") && allow)
            PlayerControl.LocalPlayer.RpcSetRole((RoleTypes)RoleId.Get<SurvivorRole>(), false);

        // ============================
        //         MODIFIERS
        // ============================
        Header($"<color=#{LaunchpadPalette.GeneralMenu.ToHtmlStringRGBA()}>MODIFIERS</color>");

        if (GUILayout.Button("Add: V.I.P") && allow)
            PlayerControl.LocalPlayer.GetModifierComponent().AddModifier<KingModifier>();

        if (GUILayout.Button("Add: Flash") && allow)
            PlayerControl.LocalPlayer.GetModifierComponent().AddModifier<FlashModifier>();

        if (GUILayout.Button("Add: Bait") && allow)
            PlayerControl.LocalPlayer.GetModifierComponent().AddModifier<BaitModifier>();

        if (GUILayout.Button("Add: Burst") && allow)
            PlayerControl.LocalPlayer.GetModifierComponent().AddModifier<BurstModifier>();

        if (GUILayout.Button("Add: Mayor") && allow)
            PlayerControl.LocalPlayer.GetModifierComponent().AddModifier<MayorModifier>();

        if (GUILayout.Button("Add: Torch") && allow)
            PlayerControl.LocalPlayer.GetModifierComponent().AddModifier<TorchModifier>();

        // ============================
        //           OTHER
        // ============================
        Header($"<color=#{LaunchpadPalette.LpMenu.ToHtmlStringRGBA()}>OTHER</color>");

        if (GUILayout.Button("Kill Player (Yourself)") && allow)
            PlayerControl.LocalPlayer.RpcCustomMurder(PlayerControl.LocalPlayer);

            GUILayout.EndScrollView();
        })
    {
        Enabled = true,
    };
    public void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            Visible = !Visible; // toggle
    }
    public void OnGUI()
    {
        if (Visible)
            DebuggingWindow.OnGUI();
    }
}
