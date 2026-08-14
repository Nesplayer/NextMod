using HarmonyLib;
using UnityEngine;
using UnityEngine.Video;
using NEXT.Features;

namespace NEXT.Patches.Misc;

[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
public static class LogoPatch
{
    public static void Postfix()
    {

        var newLogo = GameObject.Find("LOGO-AU");
        var sizer = GameObject.Find("Sizer");
        if (newLogo != null)
        {
            newLogo.GetComponent<SpriteRenderer>().sprite = LaunchpadAssets.TORWBanner.LoadAsset();
        }

        if (sizer != null)
        {
            sizer.GetComponent<AspectSize>().PercentWidth = 0.3f;
            sizer.gameObject.transform.localScale = new Vector3(0.7604f, 0.7604f, sizer.gameObject.transform.localScale.z);
        }
        
        var lpanel = GameObject.Find("LeftPanel");

        if (lpanel != null)
        {
            lpanel.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0.636f, 1f);
        }
        
        var rpanel = GameObject.Find("RightPanel");

        if (rpanel != null)
        {
            rpanel.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0.5f, 1f);
        }

        var divider = GameObject.Find("Divider");

        if (divider != null)
        {
            divider.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0.3742f, 1f);
        }

        var menuBg = GameObject.Find("BackgroundTexture");
        if (menuBg != null)
        {
            var render = menuBg.GetComponent<SpriteRenderer>();
            render.flipY = true;
            render.color = new Color(0f, 0.6348f, 0.9392f, 1f);
        }

        var tint = GameObject.Find("MainUI").transform.GetChild(0).gameObject;
        if (tint != null)
        {
            tint.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.1f);
            tint.transform.localScale = new Vector3(7.5f, 7.5f, 1f);
        }
    }
}