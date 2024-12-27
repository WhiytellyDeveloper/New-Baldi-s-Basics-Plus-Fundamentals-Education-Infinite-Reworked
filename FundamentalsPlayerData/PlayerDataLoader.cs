using nbbpfei_reworked.FundamentalsManagers;
using System.Collections.Generic;
using System.IO;
using UnityCipher;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsPlayerData
{
    public static class PlayerDataLoader
    {
        private static List<PlayerData> players = new();

        public static void LoadAllPlayers()
        {
            Debug.Log("Start loading all player data from folder:");

            string fullPath = FilePathsManager.GetPath(FilePaths.PlayerData);
            if (!Directory.Exists(fullPath))
            {
                Debug.LogError("Player data folder not found.");
                return;
            }

            string[] filePaths = Directory.GetFiles(fullPath, "*.playerData");
            foreach (string file in filePaths)
            {
                var playerData = LoadPlayer(Path.GetFileNameWithoutExtension(file));

                if (playerData != null && !IsPlayerExist(playerData.playerName))
                {
                    players.Add(playerData);
                    Debug.Log($"Loaded player: {playerData.playerName}");
                }
            }
        }

        public static PlayerData LoadPlayer(string playerName)
        {
            Debug.Log($"Attempting to load player data for: {playerName}");

            string filePath = FilePathsManager.GetPath(FilePaths.PlayerData, $"{playerName}.playerData");
            if (!File.Exists(filePath))
            {
                Debug.LogError($"Player data file not found for: {playerName}");
                return null;
            }

            string output = File.ReadAllText(filePath);
            var playerData = JsonUtility.FromJson<PlayerData>(RijndaelEncryption.Decrypt(output, ":D"));

            if (playerData != null && !IsPlayerExist(playerData.playerName))
            {
                players.Add(playerData);
                Debug.Log($"Loaded player: {playerData.playerName}");
            }

            return playerData;
        }

        public static void CreatePlayerData(string player, PlayerData preData)
        {
            var data = GetPlayer(player) ?? preData;

            data.playerName = player;

            Debug.LogError($"Creating/Updating player data for: {player}!");

            string output = JsonUtility.ToJson(data);
            string filePath = FilePathsManager.GetPath(FilePaths.PlayerData, $"{player}.playerData");
            File.WriteAllText(filePath, RijndaelEncryption.Encrypt(output, ":D"));

            if (!IsPlayerExist(player))
                players.Add(data);

            Debug.Log($"Player data saved: {player}");
        }

        public static bool IsPlayerExist(string playerName)
        {
            return players.Exists(p => p.playerName == playerName);
        }


        public static PlayerData GetPlayer(string playerName)
        {
            return players.Find(p => p.playerName == playerName);
        }

        public static PlayerData GetPlayer()
        {
            return players.Find(p => p.playerName == Singleton<PlayerFileManager>.Instance.fileName);
        }

        public static List<PlayerData> GetAllPlayers()
        {
            return new List<PlayerData>(players);
        }
    }
}
