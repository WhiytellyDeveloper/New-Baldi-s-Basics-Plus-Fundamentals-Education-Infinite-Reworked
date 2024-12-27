using KipoTupiniquimEngine.Extenssions;
using nbbpfei_reworked.FundamentalsPatchs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsManagers.Loaders
{
    internal class TextureLoader
    {
        public static void LoadCustomTextures()
        {
            Texture2D[] woodTextures = [
            AssetsHelper.LoadTexture("brightwood", "brightwood", FilePathsManager.GetPath(FilePaths.Misc, "Wood")),
            Resources.FindObjectsOfTypeAll<Texture2D>().FirstOrDefault(x => x.name == "wood 1"),
            AssetsHelper.LoadTexture("darkwood", "darkwood", FilePathsManager.GetPath(FilePaths.Misc, "Wood"))
            ];

            FundamentalsMainLoader.GetRandomFloorByName("F1").woodTextures = [(woodTextures[0].ToWeighted<WeightedTexture2D, Texture2D>(100))];
            FundamentalsMainLoader.GetRandomFloorByName("F2").woodTextures = [(woodTextures[1].ToWeighted<WeightedTexture2D, Texture2D>(100))];
            FundamentalsMainLoader.GetRandomFloorByName("F3").woodTextures = [(woodTextures[2].ToWeighted<WeightedTexture2D, Texture2D>(100))];

            Texture2D[] elvfloorTextures = [
            AssetsHelper.LoadTexture("normfloorelv", "normfloorelv", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("redfloorelv", "redfloorelv", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("bluefloorelv", "bluefloorelv", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("pinkfloorelv", "pinkfloorelv", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("brownfloorelv", "brownfloorelv", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("grayfloorelv", "grayfloorelv", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("blackfloorelv", "blackfloorelv", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
             AssetsHelper.LoadTexture("whitefloorelv", "whitefloorelv", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            ];

            Texture2D[] elvtilefloorTextures = [
            Resources.FindObjectsOfTypeAll<Texture2D>().FirstOrDefault(x => x.name == "ElFloor"),
            AssetsHelper.LoadTexture("redfloor_tile", "redfloor_tile", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("bluefloor_tile", "bluefloor_tile", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("pinkfloor_tile", "pinkfloor_tile", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("brownfloor_tile", "brownfloor_tile", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("grayfloor_tile", "grayfloor_tile", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("blackfloor_tile", "blackfloor_tile", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("whitefloor_tile", "whitefloor_tile", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            ];

            Texture2D[] elvTubesTextures = [
            AssetsHelper.LoadTexture("TransparentTubes0", "TransparentTubes0", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("TransparentTubes1", "TransparentTubes1", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("TransparentTubes2", "TransparentTubes2", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            AssetsHelper.LoadTexture("TransparentTubes3", "TransparentTubes3", FilePathsManager.GetPath(FilePaths.Misc, "Elevators")),
            ];

            ElevatorScreenPatch.elevatorSprite.AddRange(elvfloorTextures);
            ElevatorScreenPatch.elevatorTubes.AddRange(elvTubesTextures);
            ElevatorScreenPatch.elevatorFloor.AddRange(elvtilefloorTextures);
        }
    }
}
