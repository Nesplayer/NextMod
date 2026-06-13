/*
using System;
using HarmonyLib;
using MiraAPI.Events;
using MiraAPI.Events.Vanilla.Gameplay;
using MiraAPI.GameOptions;
using Reactor.Utilities.Extensions;
using TORWL.Options;
using UnityEngine;
using UnityEngine.Events;

namespace TORWL.Features.PlayerList;

public static class InGamePlayerList
{
    private static global::InGamePlayerList _plist = null!;
    private static bool _isOpen;
    public static PassiveButton PlayerListButton = null!;
    public static void CreateButton()
    {
        _plist = UnityEngine.Object.FindFirstObjectByType<global::InGamePlayerList>(FindObjectsInactive.Include);
        
        PlayerListButton = UnityEngine.Object
            .Instantiate(HudManager.Instance.SettingsButton, HudManager.Instance.SettingsButton.transform.parent).GetComponent<PassiveButton>();
        PlayerListButton.gameObject.name = "PlayerListButton";
        PlayerListButton.OnClick = new();
        PlayerListButton.OnClick.AddListener(OnClick());
        PlayerListButton.activeSprites.GetComponent<SpriteRenderer>().sprite = LaunchpadAssets.PListActive.LoadAsset();
        PlayerListButton.inactiveSprites.GetComponent<SpriteRenderer>().sprite = LaunchpadAssets.PListInactive.LoadAsset();
        PlayerListButton.GetComponent<AspectPosition>().DistanceFromEdge = new(3.61f, 0.504f, -400f);
        
        _isOpen = false;
    }
    private static Action OnClick()
    {
        void Listener()
        {
            _plist.SetActive(!_isOpen);
            _isOpen = !_isOpen;
        }

        return Listener;
    }

    [RegisterEvent]
    public static void OnGameStart(IntroBeginEvent e)
    {
        PlayerListButton.gameObject.DestroyImmediate();
        _plist.SetActive(false);
    }
    
    [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
    public static class HudStartPatch
    {
        public static void Postfix()
        {
            CreateButton();
        }
    }
}
*/