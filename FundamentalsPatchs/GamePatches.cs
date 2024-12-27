using HarmonyLib;
using KipoTupiniquimEngine.Extenssions;
using nbbpfei_reworked.FundamentalsManagers;
using nbbpfei_reworked.FundamentalsPlayerData;
using PixelInternalAPI.Extensions;
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
}
