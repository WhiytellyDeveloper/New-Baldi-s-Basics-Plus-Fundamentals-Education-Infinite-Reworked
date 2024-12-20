using EditorCustomRooms;
using MTM101BaldAPI.Registers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsManagers.Loaders
{
    internal class RoomVariantsLoader
    {
        public static void LoadCustomizedRooms()
        {
            LoadRoomVariants(RoomCategory.Faculty, 1, false, false, true, [], FilePathsManager.GetPath(FilePaths.Rooms, "Faculty", "Rooms"));
            LoadRoomVariants(RoomCategory.Class, 0, false, false, false, ["F2", "F3", "F4", "F5", "END"], FilePathsManager.GetPath(FilePaths.Rooms, "Class", "Rooms", "MathMachineClass"));
            LoadRoomVariants(RoomCategory.Class, 8, false, false, false, ["F1"], FilePathsManager.GetPath(FilePaths.Rooms, "Class", "Rooms", "NotebookClass"));
        }

        public static void LoadRoomVariant(string name, RoomCategory roomType, int roomid, bool cubic, bool keepTex, bool blueLockers, string[] overrideFloors, params string[] paths)
        {
            var roomBase = RoomAssetMetaStorage.Instance.FindAll(x => x.category == roomType)[roomid].value;

            Debug.Log($"Start Loading Room: {name} / Using {roomBase.name} As Base");

            string[] parts = name.Split('@');
            string[] firstPart = parts[0].Split(';');
            int weight = int.Parse(firstPart[0]);
            string[] floorAndName = firstPart[1].Split('!');

            string[] floors = new string[floorAndName.Length - 1];
            for (int i = 0; i < floorAndName.Length - 1; i++)
                floors[i] = floorAndName[i];

            string textureName = floorAndName[1].Replace("%", "");

            var room = RoomFactory.CreateAssetsFromPath(
                Path.Combine(Path.Combine(paths), $"{name}.cbld"),
                roomBase.maxItemValue,
                true,
                roomBase.roomFunctionContainer,
                false,
                false,
                (Texture2D)roomBase.mapMaterial.GetTexture("_MapBackground"),
                false,
                cubic
            )[0];

            //Add swap objects

            if (room.basicObjects.FirstOrDefault(x => x.prefab.name == "SodaMachine") != null)
            {
                room.basicSwaps.Add(
                new BasicObjectSwapData
                {
                    prefabToSwap = room.basicObjects.First(x => x.prefab.name == "SodaMachine").prefab,
                    potentialReplacements = new WeightedTransform[]
                    {
                        new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "DietSodaMachine"), weight = 100 },
                        new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "ZestyMachine"), weight = 80 },
                        new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "SodaMachine"), weight = 15 },
                        new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "CrazyVendingMachineBSODA"), weight = 1 },
                        new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "CrazyVendingMachineZesty"), weight = 1 },
                        new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "WaterFountain"), weight = 95 },
                    },
                    chance = 100
                });
            }

            if (room.basicObjects.Any(x => x.prefab.name == "SodaMachine") && room.basicObjects.Any(x => x.prefab.name == "WaterFountain"))
                room.basicSwaps[0].potentialReplacements = room.basicSwaps[0].potentialReplacements.Where((_, index) => index != 5).ToArray();

            if (room.basicObjects.FirstOrDefault(x => x.prefab.name == "MyComputer") != null)
            {
                room.basicSwaps.Add(
                    new BasicObjectSwapData
                    {
                        prefabToSwap = room.basicObjects.First(x => x.prefab.name == "MyComputer").prefab,
                        potentialReplacements = new WeightedTransform[]
                        {
                           new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "MyComputer_Off"), weight = 100 },
                           new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "MyComputer"), weight = 50 },
                        },
                        chance = 100
                    });
            }

            if (room.basicObjects.FirstOrDefault(x => x.prefab.name == "Bookshelf_Object") != null)
            {
                room.basicSwaps.Add(
                    new BasicObjectSwapData
                    {
                        prefabToSwap = room.basicObjects.First(x => x.prefab.name == "Bookshelf_Object").prefab,
                        potentialReplacements = new WeightedTransform[]
                        {
                           new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "Bookshelf_Object"), weight =  100 },
                           new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "Bookshelf_Hole_Object"), weight = 45 },
                        },
                        chance = 65
                    });
            }

            if (room.basicObjects.FirstOrDefault(x => x.prefab.name == "Locker") != null && blueLockers)
            {
                room.basicSwaps.Add(
                    new BasicObjectSwapData
                    {
                        prefabToSwap = room.basicObjects.First(x => x.prefab.name == "Locker").prefab,
                        potentialReplacements = new WeightedTransform[]
                        {
                           new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "BlueLocker"), weight =  30 },
                           new WeightedTransform { selection = Resources.FindObjectsOfTypeAll<Transform>().First(x => x.name == "Locker"), weight = 100 },
                        },
                        chance = 52
                    });
            }

            room.posters = roomBase.posters;
            room.posterChance = roomBase.posterChance;

            if (floors[0] == "ALL")
                floors = FundamentalsMainLoader.GetAllRandomFloorsName().ToArray();

            if (overrideFloors.Length > 0)
                floors = overrideFloors;

            Debug.Log($"Room Name: {textureName}");
            Debug.Log($"Weight: {weight}");
            Debug.Log($"Floors: [{string.Join(", ", floors)}]");

            foreach (string floor in floors)
            {
                if (FundamentalsMainLoader.GetRandomFloorByName(floor) == null)
                    return;

                FundamentalsMainLoader
                    .GetRandomFloorByName(floor)
                    .rooms[roomType]
                    .Add(new WeightedRoomAsset { selection = room, weight = weight });
            }
        }

        public static void LoadRoomVariants(RoomCategory category, int roomid, bool cubic, bool keepTex, bool blueLockers, string[] overrideFloors, params string[] path)
        {
            string fullPath = Path.Combine(path);
            if (!Directory.Exists(fullPath))
                return;

            InitializeRoomsVariants(category);

            string[] filePaths = Directory.GetFiles(fullPath);
            foreach (string file in filePaths)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                LoadRoomVariant(fileName, category, roomid, cubic, keepTex, blueLockers, overrideFloors, path);
            }
        }

        private static void InitializeRoomsVariants(RoomCategory category)
        {
            foreach (var floorData in FundamentalsMainLoader.randomFloors)
            {
                if (!floorData.rooms.ContainsKey(category))
                    floorData.rooms.Add(category, new List<WeightedRoomAsset>());
            }
        }
    }
}
