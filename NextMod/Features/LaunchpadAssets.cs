using MiraAPI.Utilities.Assets;
using Reactor.Utilities;
using TMPro;
using UnityEngine;

namespace NEXT.Features;

public static class LaunchpadAssets
{
    public static readonly AssetBundle Bundle = AssetBundleManager.Load("launchpad-assets");
    public static readonly AssetBundle Icons = AssetBundleManager.Load("icons");
    public static readonly AssetBundle Ability = AssetBundleManager.Load("ability");
    public static readonly AssetBundle Wiki = AssetBundleManager.Load("wikipanel");

    // Shaders
    public static readonly LoadableAsset<Shader> BloomShader = new LoadableBundleAsset<Shader>("Bloom.shader", Bundle);

    // Materials
    public static readonly LoadableAsset<Material> GradientMaterial = new LoadableBundleAsset<Material>("GradientPlayerMaterial", Bundle);
    public static readonly LoadableAsset<Material> MaskedGradientMaterial = new LoadableBundleAsset<Material>("MaskedGradientMaterial", Bundle);
    
    // Placeholder
    public static readonly LoadableAsset<Sprite> ImpIcon = new LoadableResourceAsset("NEXTaunchpad.Resources.Imp.png");
    public static readonly LoadableAsset<Sprite> CrewIcon = new LoadableResourceAsset("NEXTaunchpad.Resources.Crew.png");
    public static readonly LoadableAsset<Sprite> NeutIcon = new LoadableResourceAsset("NEXTaunchpad.Resources.Neutral.png");
    public static readonly LoadableAsset<Sprite> Placeholder = new LoadableResourceAsset("NEXTaunchpad.Resources.PlaceHolder.png");
	
	//Gamemode Images
	public static readonly LoadableAsset<Sprite> StopNGo = new LoadableResourceAsset("NEXTaunchpad.Resources.StopNGo.png");
	public static readonly LoadableAsset<Sprite> Outbreak = new LoadableResourceAsset("NEXTaunchpad.Resources.Outbreak.png");
	public static readonly LoadableAsset<Sprite> ComingSoon = new LoadableResourceAsset("NEXTaunchpad.Resources.ComingSoon.png");

    // Role Icons
    public static readonly LoadableAsset<Sprite> Poltergeist = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Poltergeist.png");
    public static readonly LoadableAsset<Sprite> Silencer = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Silencer.png");
    public static readonly LoadableAsset<Sprite> Toxifier = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Toxifier.png");
    public static readonly LoadableAsset<Sprite> Jester = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Jester.png");
    public static readonly LoadableAsset<Sprite> Traitor = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Traitor.png");
    public static readonly LoadableAsset<Sprite> NeutralKiller = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.NeutralKiller.png");
    public static readonly LoadableAsset<Sprite> Executioner = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Executioner.png");
    public static readonly LoadableAsset<Sprite> Medic = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Medic.png");
    public static readonly LoadableAsset<Sprite> Burrower = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Burrower.png");
    public static readonly LoadableAsset<Sprite> Coroner = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Coroner.png");
    public static readonly LoadableAsset<Sprite> Gambler = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Gambler.png");
    public static readonly LoadableAsset<Sprite> Hitman = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Hitman.png");
    public static readonly LoadableAsset<Sprite> Janitor = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Janitor.png");
    public static readonly LoadableAsset<Sprite> Reaper = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Reaper.png");
    public static readonly LoadableAsset<Sprite> Sealer = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Sealer.png");
    public static readonly LoadableAsset<Sprite> Sheriff = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Sheriff.png");
    public static readonly LoadableAsset<Sprite> Surgeon = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Surgeon.png");
    public static readonly LoadableAsset<Sprite> Swapshifter = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Swapshifter.png");
    public static readonly LoadableAsset<Sprite> TavernKeeper = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.TavernKeeper.png");
    public static readonly LoadableAsset<Sprite> Teleporter = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Teleporter.png");
    public static readonly LoadableAsset<Sprite> Survivor = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Survivor.png");
    public static readonly LoadableAsset<Sprite> Shielder = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Shielder.png");
    public static readonly LoadableAsset<Sprite> Scribe = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Scribe.png");
    public static readonly LoadableAsset<Sprite> Alchemist = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Alchemist.png");
    public static readonly LoadableAsset<Sprite> Hexbinder = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Hexbinder.png");
	public static readonly LoadableAsset<Sprite> Shroudweaver = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Shroudweaver.png");
	public static readonly LoadableAsset<Sprite> Dominator = new LoadableResourceAsset("NEXTaunchpad.Resources.RoleIcons.Dominator.png");

    // Role Buttons
    public static readonly LoadableAsset<Sprite> RoleBlockButton = new LoadableBundleAsset<Sprite>("RoleBlock.png", Icons);
	public static readonly LoadableAsset<Sprite> Shroud = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Shroud.png");
	public static readonly LoadableAsset<Sprite> Dominate = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Dominate.png");
    public static readonly LoadableAsset<Sprite> ShieldButton = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Shield.png");
    public static readonly LoadableAsset<Sprite> Cure = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Cure.png");
    public static readonly LoadableAsset<Sprite> Brew = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Brew.png");
    public static readonly LoadableAsset<Sprite> Hex = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Hex.png");
    public static readonly LoadableAsset<Sprite> SilenceButton = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Silence.png");
    public static readonly LoadableAsset<Sprite> InsightButton = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Insight.png");
    public static readonly LoadableAsset<Sprite> Toxify = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Toxify.png");
    public static readonly LoadableAsset<Sprite> Curse = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Curse.png");
    public static readonly LoadableAsset<Sprite> Vest = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.Vest.png");
    public static readonly LoadableAsset<Sprite> ReviveButton = new LoadableBundleAsset<Sprite>("Revive.png", Bundle);
    public static readonly LoadableAsset<Sprite> ShootButton = new LoadableBundleAsset<Sprite>("Shoot.png", Bundle);
    public static readonly LoadableAsset<Sprite> ZoomButton = new LoadableBundleAsset<Sprite>("Zoom.png", Bundle);
    public static readonly LoadableAsset<Sprite> SealButton = new LoadableBundleAsset<Sprite>("SealVent.png", Bundle);
    public static readonly LoadableAsset<Sprite> SwapButton = new LoadableBundleAsset<Sprite>("Swap.png", Bundle);
    public static readonly LoadableAsset<Sprite> FreezeButton = new LoadableBundleAsset<Sprite>("Freeze.png", Bundle);
    public static readonly LoadableAsset<Sprite> SoulButton = new LoadableBundleAsset<Sprite>("StealSoul.png", Bundle);
    public static readonly LoadableAsset<Sprite> GambleButton = new LoadableBundleAsset<Sprite>("Gamble.png", Bundle);
    public static readonly LoadableAsset<Sprite> DeadlockButton = new LoadableBundleAsset<Sprite>("Deadlock.png", Bundle);
    public static readonly LoadableAsset<Sprite> TeleportButton = new LoadableBundleAsset<Sprite>("TeleportButton.png", Icons);
    public static readonly LoadableAsset<Sprite> KillButton = new LoadableBundleAsset<Sprite>("Kill.png", Icons);
    public static readonly LoadableAsset<Sprite> BlankButton = new LoadableBundleAsset<Sprite>("BlankButton", Bundle);
    public static readonly LoadableAsset<Sprite> CallButton = new LoadableBundleAsset<Sprite>("CallMeeting.png", Bundle);
    public static readonly LoadableAsset<Sprite> DissectButton = new LoadableBundleAsset<Sprite>("Dissect.png", Bundle);
    public static readonly LoadableAsset<Sprite> DragButton = new LoadableBundleAsset<Sprite>("Drag.png", Bundle);
    public static readonly LoadableAsset<Sprite> DropButton = new LoadableBundleAsset<Sprite>("Drop.png", Bundle);
    public static readonly LoadableAsset<Sprite> HackButton = new LoadableBundleAsset<Sprite>("Hack.png", Bundle);
    public static readonly LoadableAsset<Sprite> HideButton = new LoadableBundleAsset<Sprite>("Clean.png", Bundle);
    public static readonly LoadableAsset<Sprite> InjectButton = new LoadableBundleAsset<Sprite>("Inject.png", Bundle);
    public static readonly LoadableAsset<Sprite> DigVentButton = new LoadableBundleAsset<Sprite>("DigVent.png", Bundle);

    // Ability Counter Sprites
    public static readonly LoadableAsset<Sprite> Basic = new LoadableBundleAsset<Sprite>("Basic.png", Ability);
    public static readonly LoadableAsset<Sprite> Player = new LoadableBundleAsset<Sprite>("Player.png", Ability);
    public static readonly LoadableAsset<Sprite> Body = new LoadableBundleAsset<Sprite>("Body.png", Ability);
    public static readonly LoadableAsset<Sprite> Vent = new LoadableBundleAsset<Sprite>("Vent.png", Ability);

    //Menu Buttons/Icons
    public static readonly LoadableAsset<Sprite> EditIcon = new LoadableResourceAsset("NEXTaunchpad.Resources.MenuIcons.Edit.png");
    public static readonly LoadableAsset<Sprite> ViewIcon = new LoadableResourceAsset("NEXTaunchpad.Resources.MenuIcons.View.png");
    public static readonly LoadableAsset<Sprite> RolesIcon = new LoadableResourceAsset("NEXTaunchpad.Resources.MenuIcons.Roles.png");
    public static readonly LoadableAsset<Sprite> ModifiersIcon = new LoadableResourceAsset("NEXTaunchpad.Resources.MenuIcons.Modifiers.png");
    public static readonly LoadableAsset<Sprite> WikiButton = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.WikiButton.png");
    public static readonly LoadableAsset<Sprite> NewsButtonIcon = new LoadableResourceAsset("NEXTaunchpad.Resources.Mega.png");
    public static readonly LoadableAsset<Sprite> ModIcon = new LoadableResourceAsset("NEXTaunchpad.Resources.Icon.png");
    
    // Banners
    public static readonly LoadableAsset<Sprite> TORWBanner = new LoadableResourceAsset("NEXTaunchpad.Resources.Banner.png");

    // CovenFile
    public static readonly LoadableAsset<Sprite> CovenFile = new LoadableResourceAsset("NEXTaunchpad.Resources.CovenFile.png");
    
    // Modifier Icons
    public static readonly LoadableAsset<Sprite> BaitIcon = new LoadableBundleAsset<Sprite>("Bait.png", Icons);
    public static readonly LoadableAsset<Sprite> FlashIcon = new LoadableBundleAsset<Sprite>("Flash.png", Icons);
    public static readonly LoadableAsset<Sprite> VIPIcon = new LoadableBundleAsset<Sprite>("VIP.png", Icons);
    public static readonly LoadableAsset<Sprite> TorchIcon = new LoadableBundleAsset<Sprite>("Torch.png", Icons);
    public static readonly LoadableAsset<Sprite> BurstIcon = new LoadableBundleAsset<Sprite>("Burst.png", Icons);
    public static readonly LoadableAsset<Sprite> MayorIcon = new LoadableBundleAsset<Sprite>("Mayor.png", Icons);
    public static readonly LoadableAsset<Sprite> Silenced = new LoadableResourceAsset("NEXTaunchpad.Resources.ModifierIcons.Silenced.png");
    public static readonly LoadableAsset<Sprite> Cursed = new LoadableResourceAsset("NEXTaunchpad.Resources.ModifierIcons.Cursed.png");
    
    // UI Buttons
    public static readonly LoadableAsset<Sprite> PListActive = new LoadableResourceAsset("NEXTaunchpad.Resources.PlayerListActive.png");
    public static readonly LoadableAsset<Sprite> PListInactive = new LoadableResourceAsset("NEXTaunchpad.Resources.PlayerListInactive.png");
    public static readonly LoadableAsset<Sprite> IconBackground = new LoadableResourceAsset("NEXTaunchpad.Resources.Background.png");
    public static readonly LoadableAsset<Sprite> FreeplayRoleButton = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.FreeplayRoleButton.png");
    public static readonly LoadableAsset<Sprite> FreeplayModifierButton = new LoadableResourceAsset("NEXTaunchpad.Resources.Buttons.FreeplayModifierButton.png");
    public static readonly LoadableAsset<Sprite> NotepadSprite = new LoadableBundleAsset<Sprite>("NotepadButton.png", Bundle);
    public static readonly LoadableAsset<Sprite> NotepadActiveSprite = new LoadableBundleAsset<Sprite>("NotepadButtonActive.png", Bundle);

    // Object Sprites
    public static readonly LoadableAsset<Sprite> Bone = new LoadableBundleAsset<Sprite>("Bone.png", Bundle);
    public static readonly LoadableAsset<Sprite> Footstep = new LoadableBundleAsset<Sprite>("Footstep.png", Bundle);
    public static readonly LoadableAsset<Sprite> VentTape = new LoadableBundleAsset<Sprite>("VentTape.png", Bundle);
    public static readonly LoadableAsset<Sprite> VentTapePolus = new LoadableBundleAsset<Sprite>("VentTapePolus.png", Bundle);
    public static readonly LoadableAsset<Sprite> KnifeHandSprite = new LoadableBundleAsset<Sprite>("KnifeHand.png", Bundle);
    public static readonly LoadableAsset<Sprite> NodeSprite = new LoadableBundleAsset<Sprite>("Node.png", Bundle);
    
    public static readonly LoadableAsset<Sprite> DeadlockHonor = new LoadableBundleAsset<Sprite>("DeadlockHonor.png", Bundle);
    public static readonly LoadableAsset<Sprite> DeadlockTarget = new LoadableBundleAsset<Sprite>("DeadlockTarget.png", Bundle);
    public static readonly LoadableAsset<Sprite> DeadlockVignette = new LoadableBundleAsset<Sprite>("DeadlockVignette.png", Bundle);
    public static readonly LoadableAsset<Sprite> FrozenBodyOverlay = new LoadableBundleAsset<Sprite>("BodyFrozenOverlay.png", Bundle);

    // Sounds
    public static readonly LoadableAsset<AudioClip> BeepSound = new LoadableBundleAsset<AudioClip>("Beep.wav", Bundle);
    public static readonly LoadableAsset<AudioClip> InjectSound = new LoadableBundleAsset<AudioClip>("Inject.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> DissectSound = new LoadableBundleAsset<AudioClip>("Dissect.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> HackingSound = new LoadableBundleAsset<AudioClip>("HackAmbience.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> PingSound = new LoadableBundleAsset<AudioClip>("Ping.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> MoneySound = new LoadableBundleAsset<AudioClip>("MoneySound.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> BuzzerSound = new LoadableBundleAsset<AudioClip>("Buzzer.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> DigSound = new LoadableBundleAsset<AudioClip>("Dig.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> TapeSound = new LoadableBundleAsset<AudioClip>("Tape.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> SwooshSound = new LoadableBundleAsset<AudioClip>("Swoosh.mp3", Bundle);
    public static readonly LoadableAsset<AudioClip> ReaperSound = new LoadableBundleAsset<AudioClip>("CollectSoul.mp3", Bundle);

    // Deadlock Sounds
    public static readonly LoadableAsset<AudioClip> DeadlockAmbience = new LoadableBundleAsset<AudioClip>("DeadlockAmbience.wav", Bundle);
    public static readonly LoadableAsset<AudioClip> DeadlockFadeIn = new LoadableBundleAsset<AudioClip>("DeadlockFadeIn.wav", Bundle);
    public static readonly LoadableAsset<AudioClip> DeadlockFadeOut = new LoadableBundleAsset<AudioClip>("DeadlockFadeOut.wav", Bundle);
    public static readonly LoadableAsset<AudioClip> DeadlockClockLeft = new LoadableBundleAsset<AudioClip>("DeadlockClockLeft.wav", Bundle);
    public static readonly LoadableAsset<AudioClip> DeadlockClockRight = new LoadableBundleAsset<AudioClip>("DeadlockClockRight.wav", Bundle);
    public static readonly LoadableAsset<AudioClip> DeadlockMark = new LoadableBundleAsset<AudioClip>("DeadlockMark.wav", Bundle);
    public static readonly LoadableAsset<AudioClip> DeadlockKillConfirmal = new LoadableBundleAsset<AudioClip>("DeadlockKillConfirmal.wav", Bundle);

    // Other
    public static readonly LoadableAsset<GameObject> DetectiveGame = new LoadableBundleAsset<GameObject>("JournalMinigame", Bundle);
    public static readonly LoadableAsset<GameObject> RoleMinigame = new LoadableBundleAsset<GameObject>("RoleGuessingMinigame", Bundle);
    public static readonly LoadableAsset<GameObject> ReaperDisplay = new LoadableBundleAsset<GameObject>("ReaperSoulDisplay", Bundle);
    public static readonly LoadableAsset<GameObject> Notepad = new LoadableBundleAsset<GameObject>("Notepad", Bundle);
    public static readonly LoadableAsset<GameObject> NodeGame = new LoadableBundleAsset<GameObject>("NodeMinigame", Bundle);
    public static readonly LoadableAsset<GameObject> WikiPrefab = new LoadableBundleAsset<GameObject>("WikiPanel", Wiki);

    public static readonly LoadableAsset<GameObject> PlayerTags = new LoadableBundleAsset<GameObject>("PlayerTags", Bundle);
}