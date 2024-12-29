using nbbpfei_reworked.FundamentalsManagers.Loaders;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsManagers
{
    public static class FundamentalsMainLoader
    {
        public static void LoadFundamentalsMod()
        {
            TileTextureLoader.LoadTileTextures();
            TextureLoader.LoadCustomTextures();
            RoomFunctionLoader.LoadRoomFunctions();
            MusicalLoader.LoadFundamentalsMusical();
            RoomVariantsLoader.LoadCustomizedRooms();
            CompatibilityLoader.LoadCompatibility();
            //OldRoomCreator.Load(); - i hate you >:(
        }

        public static RandomlyFloorData GetRandomFloorByName(string name) {
            return randomFloors.FirstOrDefault(x => x.floorId == name);
        }

        public static PreMadeFloorData GetPreMadeFloorByName(string name) {
            return preMadeFloors.FirstOrDefault(x => x.floorId == name);
        }

        public static List<string> GetAllRandomFloorsName()
        {
            List<string> list = [];

            foreach (RandomlyFloorData floor in randomFloors)
                list.Add(floor.floorId);

            return list;
        }

        public static RandomlyFloorData[] randomFloors = [new("F1"), new("F2"), new("F3"), new("F4"), new("F5"), new("END")];
        public static PreMadeFloorData[] preMadeFloors = [new("END"), new("C1"), new("C2"), new("C3")];
    }

    public class FloorData
    {
        public FloorData(string name) =>
            floorId = name;

        public string floorId { get; set; }

        public string schoolhouseMusic, escapeMusic;

    }

    public class RandomlyFloorData : FloorData
    {
        public RandomlyFloorData(string name) : base(name) =>
            floorId = name;

        public Dictionary<RoomCategory, List<WeightedTexture2D>> wallTextures = [];
        public Dictionary<RoomCategory, List<WeightedTexture2D>> floorTextures = [];
        public Dictionary<RoomCategory, List<WeightedTexture2D>> ceilingTextures = [];

        public Dictionary<RoomCategory, List<WeightedRoomAsset>> rooms = [];

        public WeightedTexture2D[] woodTextures = [];

        public string pitStopMusic;
    }

    public class PreMadeFloorData : FloorData
    {
        public PreMadeFloorData(string name) : base(name) =>
            floorId = name;

        public Dictionary<RoomCategory, Texture2D> wallTextures = [];
        public Dictionary<RoomCategory, Texture2D> floorTextures = [];
        public Dictionary<RoomCategory, Texture2D> ceilingTextures = [];

        public Texture2D woodTextue = Resources.FindObjectsOfTypeAll<Texture2D>().FirstOrDefault(x => x.name == "wood");

        public string pitStopMusic;
    }
    public static class GenericStuff
    {
        public static SoundObject xylophoneSoundGeneric = AssetsHelper.LoadSound("Xylophone", "XylophoneSound", SoundType.Effect, Color.white, "", FilePathsManager.GetPath(FilePaths.Misc, ""));
    }

}
