using AmongUs.GameOptions;
using HarmonyLib;
using MiraAPI.Roles;
using MiraAPI.Utilities.Assets;
using Reactor.Utilities.Extensions;
using UnityEngine;

namespace NEXT.Patches
{
    public static class RoleIcons
    {
        public static bool CanLocalPlayerSeeRole(RoleBehaviour role)
        {
            if (PlayerControl.LocalPlayer?.Data?.Role == null) return false;

            // Always show your own role
            if (role.Player == PlayerControl.LocalPlayer) return true;

            // Custom role from any mod — delegate to MiraAPI's visibility check
            if (role is ICustomRole customRole)
                return customRole.CanLocalPlayerSeeRole(PlayerControl.LocalPlayer);

            // Vanilla: impostors see each other, dead players see everyone
            return (PlayerControl.LocalPlayer.Data.Role.IsImpostor && role.Player.Data.Role.IsImpostor)
                || PlayerControl.LocalPlayer.Data.IsDead;
        }

        public static void ApplyRoleIcon(PlayerControl player)
        {
            if (player?.Data?.Role == null) return;
            if (player.cosmetics?.nameTextContainer == null) return;

            TryClearRoleIcon(player);

            RoleBehaviour role = player.Data.Role;
            if (!CanLocalPlayerSeeRole(role)) return;

            Sprite? roleIcon = GetRoleIcon(role);
            if (roleIcon == null)
            {
                Debug.LogWarning($"[RoleIcons] No icon found for {role.name} on {player.name}");
                return;
            }

            var go = new GameObject("RoleIcon");
            go.transform.SetParent(player.cosmetics.nameTextContainer.transform, false);

            var nameText = player.cosmetics.nameText;
            float nameWidth = nameText.preferredWidth / 2f; // half since text is centered
            go.transform.localPosition = new Vector3(-(nameWidth + 0.2f), 0f, -1f);

            go.transform.localScale = GetRoleIconScale(roleIcon, role is ICustomRole, role);

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = roleIcon;
            sr.sortingOrder = 10;
        }

        public static void ApplyAllIcons()
        {
            foreach (var player in PlayerControl.AllPlayerControls)
            {
                if (player != null) ApplyRoleIcon(player);
            }
        }

        private static Vector3 GetRoleIconScale(Sprite icon, bool isCustomRole, RoleBehaviour role)
        {
            if (role.Role is RoleTypes.Crewmate or RoleTypes.Impostor)
                return new Vector3(0.2f, 0.2f, 1f);

            float targetSize = isCustomRole ? 0.3f : 1.2f;
            float spriteSize = (float)icon.texture.height / icon.pixelsPerUnit;
            float scale = targetSize / spriteSize;
            return new Vector3(scale, scale, 1f);
        }

        public static void TryClearRoleIcon(PlayerControl player)
        {
            var t = player.cosmetics.nameTextContainer.transform.FindChild("RoleIcon");
            if (t != null) t.gameObject.Destroy();
        }

        public static Sprite? GetRoleIcon(RoleBehaviour role)
        {
            if (role is ICustomRole customRole)
                return customRole.Configuration.Icon?.LoadAsset();
            return role.RoleIconSolid;
        }

        // Fires when the HUD starts — game is fully loaded at this point
        [HarmonyPatch(typeof(HudManager), nameof(HudManager.Start))]
        public static class HudStartPatch
        {
            public static void Postfix()
            {
                Debug.Log("[RoleIcons] HudManager.Start fired, applying all icons");
                ApplyAllIcons();
            }
        }

        // Fires whenever any player's role behaviour is set directly
        [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.RpcSetRole))]
        public static class SetRolePatch
        {
            public static void Postfix(PlayerControl __instance)
            {
                Debug.Log($"[RoleIcons] SetRole fired for {__instance.name}");
                ApplyRoleIcon(__instance);
            }
        }

        [HarmonyPatch(typeof(Vent), nameof(Vent.EnterVent))]
        public static class EnterVentPatch
        {
            public static void Postfix(PlayerControl pc)
            {
                var t = pc?.cosmetics?.nameTextContainer?.transform.FindChild("RoleIcon");
                if (t != null) t.gameObject.SetActive(false);
            }
        }

        [HarmonyPatch(typeof(Vent), nameof(Vent.ExitVent))]
        public static class ExitVentPatch
        {
            public static void Postfix(PlayerControl pc)
            {
                var t = pc?.cosmetics?.nameTextContainer?.transform.FindChild("RoleIcon");
                if (t != null) t.gameObject.SetActive(true);
            }
        }
    }
}