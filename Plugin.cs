using BepInEx;
using HarmonyLib;
using MTM101BaldAPI;
using MTM101BaldAPI.OptionsAPI;
using MTM101BaldAPI.Registers;
using MTM101BaldAPI.SaveSystem;
using nbbpfei_reworked.FundamentalsManagers;
using nbbpfei_reworked.FundamentalsManagers.Loaders;
using nbbpfei_reworked.FundamentalsOptions;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace nbbpfei_reworked
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("mtm101.rulerp.bbplus.baldidevapi", MTM101BaldiDevAPI.VersionNumber)]
    [BepInProcess("BALDI.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance { get; private set; }

        private void Awake()
        {
            Harmony harmony = new(PluginInfo.PLUGIN_GUID);
            instance = this;
            harmony.PatchAllConditionals();

            LoadingEvents.RegisterOnAssetsLoaded(Info, ModPreLoading(), false);
            LoadingEvents.RegisterOnAssetsLoaded(Info, ModPostLoading(), true);

            ModdedSaveGame.AddSaveHandler(Info);

            GeneratorManagement.Register(this, GenerationModType.Base, Share);

            CustomOptionsCore.OnMenuInitialize += (optInstance, handler) => handler.AddCategory<BetaOpitions>("Fundamentals\nOptions");
        }

        public void Share(string floorName, int floorNum, SceneObject scene)
        {
            var level = scene.levelObject;
            var levelData = FundamentalsMainLoader.GetRandomFloorByName(floorName);

            level.hallWallTexs = level.hallWallTexs.AddRangeToArray(levelData.wallTextures[RoomCategory.Hall].ToArray());
            level.hallFloorTexs = level.hallFloorTexs.AddRangeToArray(levelData.floorTextures[RoomCategory.Hall].ToArray());
            level.hallCeilingTexs = level.hallCeilingTexs.AddRangeToArray(levelData.ceilingTextures[RoomCategory.Hall].ToArray());

            foreach (RoomGroup group in level.roomGroup)
            {
                var type = group.potentialRooms[0].selection.category;

                if (levelData.wallTextures.ContainsKey(type)) group.wallTexture = group.wallTexture.AddRangeToArray(levelData.wallTextures[type].ToArray());
                if (levelData.floorTextures.ContainsKey(type)) group.floorTexture = group.floorTexture.AddRangeToArray(levelData.floorTextures[type].ToArray());
                if (levelData.ceilingTextures.ContainsKey(type)) group.ceilingTexture = group.ceilingTexture.AddRangeToArray(levelData.ceilingTextures[type].ToArray());
                if (levelData.rooms.ContainsKey(type)) group.potentialRooms = group.potentialRooms.AddRangeToArray(levelData.rooms[type].ToArray());
            }
        }

        public void ShareWithPreMadeMaps()
        {

            foreach (SceneObject scene in Resources.FindObjectsOfTypeAll<SceneObject>())
            {
                string floorName = scene.levelTitle;

                if (scene.levelObject != null && floorName == "???")
                    return;

                Debug.LogError(floorName);
                var level = scene.levelAsset;
                var levelData = FundamentalsMainLoader.GetPreMadeFloorByName(floorName);

                try
                {
                    foreach (RoomData roomData in level.rooms)
                    {
                        var type = roomData.category;

                        if (levelData.wallTextures.ContainsKey(type)) roomData.wallTex = levelData.wallTextures[type];
                        if (levelData.floorTextures.ContainsKey(type)) roomData.florTex = levelData.floorTextures[type];
                        if (levelData.ceilingTextures.ContainsKey(type)) roomData.ceilTex = levelData.ceilingTextures[type];
                    }
                }
                catch { 
                }
            }
        }


        private IEnumerator ModPreLoading()
        {
            yield return 1;
            yield return "Fundamentals Pre Loading...";

            FundamentalsMainLoader.LoadFundamentalsMod();
            //TileTextureLoader.LoadTileTexture("Wall@75;F1!F2!F3!F4!F5!END!OrangeBrickWall%", RoomCategory.Hall, []);
        }

        private IEnumerator ModPostLoading()
        {
            yield return 1;
            yield return "Fundamentals Post Loading...";
            ShareWithPreMadeMaps();

            //BBTimes.ModPatches.EnvironmentPatches.EnvironmentControllerMakeBeautifulOutside.CoverEmptyWallsFromOutside = true;

        }
    }

    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "whiytellydeveloper.plugin.mod.newbaldibasicspluseducationalfundamentalsinfinite";
        public const string PLUGIN_NAME = "New Baldi's Basics Plus Fundamentals Education Infinite";
        public const string PLUGIN_VERSION = "0.1";
    }
}
