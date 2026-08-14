using HarmonyLib;
using Reactor.Utilities.Extensions;
using UnityEngine;
using System.Collections.Generic;
using NEXT.Components;

namespace NEXT.Patches
{
    public static class CommsSabotagePatch
    {
        // Store original cosmetic data per player
        private static readonly Dictionary<int, CosmeticData> OriginalCosmetics = new();

        private struct CosmeticData
        {
            public string HatId;
            public string PetId;
            public string VisorId;
            public string SkinId;
            public string NameText;
            public Color NameColor;
            public int PrimaryColor;
            public int SecondaryColor;
            public string ColorBlindName;
        }

        public static bool CommsActive { get; private set; }

        public static void OnCommsSabotaged()
        {
            if (CommsActive) return;
            CommsActive = true;

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player == null || player.Data == null) continue;
                if (player.Data.IsDead) continue;

                SaveAndStripPlayer(player);
            }
        }

        public static void OnCommsFixed()
        {
            if (!CommsActive) return;
            CommsActive = false;

            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player == null || player.Data == null) continue;
                if (player.Data.IsDead) continue;

                RestorePlayer(player);
            }

            OriginalCosmetics.Clear();
        }

        private static void SaveAndStripPlayer(PlayerControl player)
        {
            int id = player.PlayerId;

            var gradient = player.cosmetics.currentBodySprite.BodySprite
                .GetComponent<GradientColorComponent>();

            OriginalCosmetics[id] = new CosmeticData
            {
                HatId = player.Data.DefaultOutfit.HatId,
                VisorId = player.Data.DefaultOutfit.VisorId,
                SkinId = player.Data.DefaultOutfit.SkinId,
                NameText = player.Data.PlayerName,
                NameColor = player.cosmetics.nameText.color,
                PrimaryColor = gradient?.primaryColor ?? player.Data.DefaultOutfit.ColorId,
                SecondaryColor = gradient?.secondaryColor ?? player.Data.DefaultOutfit.ColorId,
                ColorBlindName = player.cosmetics.colorBlindText.text,
            };

            player.cosmetics.SetHat("", player.Data.DefaultOutfit.ColorId);
            player.cosmetics.SetVisor("", player.Data.DefaultOutfit.ColorId);
            player.cosmetics.SetSkin("", player.Data.DefaultOutfit.ColorId);
            player.cosmetics.SetPetVisible(false);

            if (gradient != null)
                gradient.SetColor(Color.gray, Color.gray);
            else
                player.RawSetColor(15);

            player.cosmetics.nameText.text = "???";
            player.cosmetics.nameText.color = Color.gray;
            
            player.cosmetics.colorBlindText.text = "???";

            RoleIcons.TryClearRoleIcon(player);
        }

        private static void RestorePlayer(PlayerControl player)
        {
            int id = player.PlayerId;
            if (!OriginalCosmetics.TryGetValue(id, out var data)) return;

            int colorId = player.Data.DefaultOutfit.ColorId;

            player.cosmetics.SetHat(data.HatId, colorId);
            player.cosmetics.SetVisor(data.VisorId, colorId);
            player.cosmetics.SetSkin(data.SkinId, colorId);
            player.cosmetics.SetPetVisible(true);

            var gradient = player.cosmetics.currentBodySprite.BodySprite
                    .GetComponent<GradientColorComponent>();
                if (gradient != null)
                    gradient.SetColor(gradient.primaryColor, gradient.secondaryColor);
                else
                    player.RawSetColor(colorId);

            player.cosmetics.nameText.text = data.NameText;
            player.cosmetics.nameText.color = data.NameColor;
            
            player.cosmetics.colorBlindText.text = data.ColorBlindName;

            RoleIcons.ApplyRoleIcon(player);
        }

        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.FixedUpdate))]
        public static class CommsSabotageCheckPatch
        {
            private static bool _lastCommsState;

            public static void Postfix()
            {
                if (ShipStatus.Instance == null) return;

                bool commsNowSabotaged = ShipStatus.Instance.Systems.ContainsKey(SystemTypes.Comms) &&
                    ShipStatus.Instance.Systems[SystemTypes.Comms].Cast<HudOverrideSystemType>()?.IsActive == true;

                if (commsNowSabotaged == _lastCommsState) return;
                _lastCommsState = commsNowSabotaged;

                if (commsNowSabotaged)
                    OnCommsSabotaged();
                else
                    OnCommsFixed();
            }
        }
    }
}