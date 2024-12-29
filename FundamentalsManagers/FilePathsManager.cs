using MTM101BaldAPI.AssetTools;
using System.Collections.Generic;
using System.IO;

namespace nbbpfei_reworked.FundamentalsManagers
{
    public static class FilePathsManager
    {
        public static Dictionary<FilePaths, string> paths = new Dictionary<FilePaths, string> {
            { FilePaths.Rooms, "Rooms" },
            { FilePaths.Musics, "Musics" },
            { FilePaths.Misc, "Misc" },
            { FilePaths.PlayerData, "PlayerData" },
            { FilePaths.Items, "Items" }
        };


        public static string GetPath(FilePaths path, params string[] strings) {
            return Path.Combine(AssetLoader.GetModPath(Plugin.instance), paths[path], Path.Combine(strings));
        }
    }

    public enum FilePaths
    {
        Rooms,
        Musics,
        Misc,
        PlayerData,
        Items
    }
}
