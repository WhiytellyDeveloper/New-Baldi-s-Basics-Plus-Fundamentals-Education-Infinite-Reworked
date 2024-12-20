using MTM101BaldAPI.Registers;
using System.IO;
using System.Linq;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsManagers.Loaders
{
    internal class TileTextureLoader
    {
        public static void LoadTileTextures()
        {
            LoadTileTextureForRoom(RoomCategory.Hall, FilePathsManager.GetPath(FilePaths.Rooms, ["Hall", "Textures"]));
            LoadTileTextureForRoom(RoomCategory.Class, FilePathsManager.GetPath(FilePaths.Rooms, ["Class", "Textures"]));
            LoadTileTextureForRoom(RoomCategory.Faculty, FilePathsManager.GetPath(FilePaths.Rooms, ["Faculty", "Textures"]));
            LoadTileTextureForRoom(RoomCategory.Office, FilePathsManager.GetPath(FilePaths.Rooms, ["Office", "Textures"]));
            CopyTexturesForRoom(RoomCategory.Hall, RoomCategory.Class);
            CopyTexturesForRoom(RoomCategory.Hall, RoomCategory.Faculty, "Wall");
            CopyTexturesForRoom(RoomCategory.Faculty, RoomCategory.Office);

            //PreMade Rooms
            LoadTileTextureForRoom("Cafeteria", RoomCategory.Special, FilePathsManager.GetPath(FilePaths.Rooms, ["Cafeteria", "Textures"]));
            LoadTileTextureForRoom("Library", RoomCategory.Special, FilePathsManager.GetPath(FilePaths.Rooms, ["Library", "Textures"]));
            LoadTileTextureForRoom("Store",RoomCategory.Store, FilePathsManager.GetPath(FilePaths.Rooms, ["JohnnyStore", "Textures"]));

            //PreMade Floors
            SetRoomTexturesForPreMadeFloor("END", RoomCategory.Hall, "GenericBrickWall", "WhiytellyHouseCarpetBlue", "NyanCeiling");            
            SetRoomTexturesForPreMadeFloor("END", RoomCategory.Class, "BrickWillyWhite", "Carpet_UpBlue", "GenericCeiling1");
            SetRoomTexturesForPreMadeFloor("END", RoomCategory.Faculty, "OfficeHalfWall", "Calpert_Pink", "HapracoNewCeiling");
            SetRoomTexturesForPreMadeFloor("END", RoomCategory.Office, "OfficeSaloonWl", "7u19waCarpet", "HapracoCeiling");

            SetRoomTexturesForPreMadeFloor("C3", RoomCategory.Hall, "GenericBrickWall", "WhiytellyHouseCarpetBlue", "NyanCeiling");
            SetRoomTexturesForPreMadeFloor("C3", RoomCategory.Class, "BrickWillyWhite", "Carpet_UpBlue", "GenericCeiling1");
            SetRoomTexturesForPreMadeFloor("C3", RoomCategory.Faculty, "OfficeHalfWall", "Calpert_Pink", "HapracoNewCeiling");
            SetRoomTexturesForPreMadeFloor("C3", RoomCategory.Office, "OfficeWallBlack", "7u19waCarpet", "HapracoCeiling");
        }

        public static void LoadTileTexture(string name, RoomCategory roomType, params string[] paths)
        {
            Debug.Log("Start Loading Texture:" + name);
            string[] parts = name.Split('@');
            string textureType = parts[0];
            string[] secondPart = parts[1].Split(';');
            int weight = int.Parse(secondPart[0]);
            string[] floorAndName = secondPart[1].Split('!');

            string[] floors = new string[floorAndName.Length - 1];
            for (int i = 0; i < floorAndName.Length - 1; i++)
                floors[i] = floorAndName[i];

            string textureName = floorAndName[floorAndName.Length - 1].Replace("%", "");

            var texture = AssetsHelper.LoadTexture(name, textureName, paths);

            if (floors[0] == "ALL")
                floors = FundamentalsMainLoader.GetAllRandomFloorsName().ToArray();

            Debug.Log("Texture Name: " + textureName);
            Debug.Log("Texture Type: " + textureType);
            Debug.Log("Weight: " + weight);
            Debug.Log("Floors: [" + string.Join(", ", floors) + "]");

            foreach (string floor in floors)
            {
                switch (textureType)
                {
                    case "Wall":
                        FundamentalsMainLoader.GetRandomFloorByName(floor).wallTextures[roomType].Add(new WeightedTexture2D{ selection = texture, weight = weight });
                        break;
                    case "Floor":
                        FundamentalsMainLoader.GetRandomFloorByName(floor).floorTextures[roomType].Add(new WeightedTexture2D { selection = texture, weight = weight });
                        break;
                    case "Ceiling":
                        FundamentalsMainLoader.GetRandomFloorByName(floor).ceilingTextures[roomType].Add(new WeightedTexture2D { selection = texture, weight = weight });
                        break;
                    default:
                        break;
                }
            }

        }

        public static void LoadTileTextureForRoom(RoomCategory category, params string[] path)
        {
            if (!Directory.Exists(Path.Combine(path)))
                return;

            InitializeRoomTexture(category);
            string[] filePaths = Directory.GetFiles(Path.Combine(path));

            foreach (string file in filePaths)
            {
                Debug.Log(Path.GetFileNameWithoutExtension(file));
                LoadTileTexture(Path.GetFileNameWithoutExtension(file), category, path);
            }
        }

        public static void LoadTileTextureForRoom(string roomAssetName, RoomCategory category, params string[] path)
        {
            var placeholder = RoomAssetMetaStorage.Instance.Get(RoomCategory.Faculty).value;

            if (!Directory.Exists(Path.Combine(path)))
                return;

            var wall = placeholder.wallTex;
            var floor = placeholder.florTex;
            var ceiling = placeholder.ceilTex;

            string[] filePaths = Directory.GetFiles(Path.Combine(path));

            foreach (string file in filePaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                Debug.Log(fileName);

                if (fileName.Contains("W!"))
                    wall = AssetsHelper.LoadTexture(fileName, fileName.Replace("W!", ""), path);

                if (fileName.Contains("F!"))
                    floor = AssetsHelper.LoadTexture(fileName, fileName.Replace("F!", ""), path);

                if (fileName.Contains("C!"))
                    ceiling = AssetsHelper.LoadTexture(fileName, fileName.Replace("C!", ""), path);
            }

            foreach (RoomAsset room in Resources.FindObjectsOfTypeAll<RoomAsset>())
            {
                if (room.category == category)
                {
                    if (room.name.Contains(roomAssetName) || roomAssetName == "")
                    {
                        if (wall != placeholder.wallTex) room.wallTex = wall;
                        if (floor != placeholder.florTex) room.florTex = floor;
                        if (ceiling != placeholder.ceilTex) room.ceilTex = ceiling;

                        Debug.Log($"Texture changes applied to the room:{room.name}\nWall:{room.wallTex.name}\nFloor:{room.florTex.name}\nCeiling:{room.ceilTex.name};");
                    }
                }
            }
        }
        public static void CopyTexturesForRoom(RoomCategory originalTexture, RoomCategory toCopyTextures, params string[] excludeParts)
        {
            foreach (var floorData in FundamentalsMainLoader.randomFloors)
            {
                if (!excludeParts.Contains("Wall"))
                    floorData.wallTextures[toCopyTextures].AddRange(floorData.wallTextures[originalTexture]);

                if (!excludeParts.Contains("Floor"))
                    floorData.floorTextures[toCopyTextures].AddRange(floorData.floorTextures[originalTexture]);

                if (!excludeParts.Contains("Ceiling"))
                    floorData.ceilingTextures[toCopyTextures].AddRange(floorData.ceilingTextures[originalTexture]);
            }
        }

        private static void InitializeRoomTexture(RoomCategory category)
        {
            foreach (var floorData in FundamentalsMainLoader.randomFloors)
            {
                if (!floorData.wallTextures.ContainsKey(category))
                    floorData.wallTextures.Add(category, new());

                if (!floorData.floorTextures.ContainsKey(category))
                    floorData.floorTextures.Add(category, new());

                if (!floorData.ceilingTextures.ContainsKey(category))
                    floorData.ceilingTextures.Add(category, new());
            }
        }

        public static void SetRoomTexturesForPreMadeFloor(string floorName, RoomCategory category, string wall, string floor, string ceiling)
        {
            FundamentalsMainLoader.GetPreMadeFloorByName(floorName).wallTextures.Add(category, AssetsHelper.Get<Texture2D>(wall));
            FundamentalsMainLoader.GetPreMadeFloorByName(floorName).floorTextures.Add(category, AssetsHelper.Get<Texture2D>(floor));
            FundamentalsMainLoader.GetPreMadeFloorByName(floorName).ceilingTextures.Add(category, AssetsHelper.Get<Texture2D>(ceiling));
        }
    }
}
