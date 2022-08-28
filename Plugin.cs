using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using ULTRAKILL;

namespace Fresh
{
    [BepInPlugin("zed.fresh", "Fresh", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Difficulty difficulty;
        ConfigEntry<Difficulty> cfg;
        Harmony harm;

        private void Awake()
        {
            Logger.LogInfo("Fresh");
            cfg = Config.Bind<Difficulty>("Difficulty","Difficulty",Difficulty.Easy,"Difficulty of the mod");
            difficulty = cfg.Value;
            harm = Harmony.CreateAndPatchAll(typeof(Plugin));
        }
        public static void Kill(bool isDead)
        {
            if(!isDead)
            {
                MonoSingleton<NewMovement>.Instance.GetHurt(200,false,1,true);
            }
        }
        
        [HarmonyPatch(typeof(StyleHUD),"UpdateFreshnessSlider")]
        [HarmonyPostfix]
        static void Patch(StyleHUD __instance)
        {
            Plugin p = new Plugin();
            NewMovement nm = MonoSingleton<NewMovement>.Instance;
            string s = __instance.freshnessSliderText.text;
            if(difficulty == Difficulty.Easy)
            {
                if(s.Contains("DULL"))
                {
                    Kill(nm.dead);
                }
            }
            else if(difficulty == Difficulty.Normal)
            {
                if(s.Contains("STALE"))
                {
                    Kill(nm.dead);
                }
            }
            else if(difficulty == Difficulty.Hard)
            {
                if(s.Contains("USED"))
                {
                    Kill(nm.dead);
                }
            }
        }
    }
}
public enum Difficulty
{
    Disabled,
    Easy,
    Normal,
    Hard
}
