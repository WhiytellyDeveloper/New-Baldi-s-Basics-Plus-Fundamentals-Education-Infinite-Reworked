using HarmonyLib;
using MTM101BaldAPI.Reflection;
using nbbpfei_reworked.FundamentalsPlayerData;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsPatchs
{

    [HarmonyPatch(typeof(GameCamera), "LateUpdate")]
    public class VertCameraPatch
    {
        [HarmonyPrefix]
        public static void Prefix(GameCamera __instance)
        {
            var value = PlayerDataLoader.GetPlayer().freeCamera;
            __instance.GetComponent<VertCam>().movementEnabled = value;
            __instance.GetComponent<VertCam>().enabled = true;
        }
    }

    [HarmonyPatch(typeof(BaseGameManager), nameof(BaseGameManager.LoadNextLevel))]
    public class RandomSeedPatch
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            if (PlayerDataLoader.GetPlayer().universeParallel)
                Singleton<CoreGameManager>.Instance.SetRandomSeed();
        }
    }

    [HarmonyPatch(typeof(Pickup), nameof(Pickup.ClickableSighted))]
    public class PickupPatch
    {
        [HarmonyPrefix]
        public static void Prefix(Pickup __instance) =>
            __instance.showDescription = PlayerDataLoader.GetPlayer().itemInformation && __instance.item.descKey != "Desc_Store";
        
    }

    [HarmonyPatch(typeof(Map), "Update")]
    internal class FullMinimapInSmallMinimap
    {
        static void Postfix(Map __instance)
        {
            if (!Singleton<CoreGameManager>.Instance.MapOpen)
            {
                if (Singleton<CoreGameManager>.Instance.GetCamera(0) == null)
                    return;

                if (!PlayerDataLoader.GetPlayer().fastMinimapIcons)
                {
                    Shader.DisableKeyword("_KEYMAPSHOWBACKGROUND");
                    return;
                }

                if (Singleton<CoreGameManager>.Instance.GetCamera(0).QuickMapAvailable)
                    Shader.EnableKeyword("_KEYMAPSHOWBACKGROUND");
            }
            else
                Shader.EnableKeyword("_KEYMAPSHOWBACKGROUND");
        }
    }

}
