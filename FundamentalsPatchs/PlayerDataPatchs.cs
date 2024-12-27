using HarmonyLib;
using nbbpfei_reworked.FundamentalsOptions;
using nbbpfei_reworked.FundamentalsPlayerData;

namespace nbbpfei_reworked.FundamentalsPatchs
{
    [HarmonyPatch]
    public static class PlayerDataPatches
    {
        [HarmonyPatch(typeof(NameManager), nameof(NameManager.Load))]
        [HarmonyPrefix]
        public static void PrefixLoad() =>
            PlayerDataLoader.LoadAllPlayers();
        

        [HarmonyPatch(typeof(NameManager), nameof(NameManager.NameClicked))]
        [HarmonyPostfix]
        public static void PostfixNameClicked()
        {
            var fileName = Singleton<PlayerFileManager>.Instance.fileName;

            if (!PlayerDataLoader.IsPlayerExist(fileName))
                PlayerDataLoader.CreatePlayerData(fileName, new());
        }
    }
}
