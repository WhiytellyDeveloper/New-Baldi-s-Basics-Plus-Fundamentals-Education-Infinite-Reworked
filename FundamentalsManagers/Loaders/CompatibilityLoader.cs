using BepInEx.Bootstrap;
using System.Linq;
using UnityEngine;
using MTM101BaldAPI.AssetTools;

namespace nbbpfei_reworked.FundamentalsManagers.Loaders
{
    internal class CompatibilityLoader
    {
        public static void LoadCompatibility()
        {
            hasTimes = Chainloader.PluginInfos.ContainsKey("pixelguy.pixelmodding.baldiplus.bbextracontent");

            if (hasTimes)
                AssetLoader.ReplaceTexture(Resources.FindObjectsOfTypeAll<Texture2D>().FirstOrDefault(x => x.name == "wallFadeInBlack"), AssetsHelper.LoadTexture("cafeteriawallFade", "cafeteriawallFade", FilePathsManager.GetPath(FilePaths.Rooms, ["Cafeteria", "Textures"])));
        }

        public static bool hasTimes = false;
    }
}
