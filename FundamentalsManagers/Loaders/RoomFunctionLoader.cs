using MTM101BaldAPI;
using MTM101BaldAPI.Reflection;
using MTM101BaldAPI.Registers;
using nbbpfei_reworked.FundamentalsContent.RoomFunctions;
using PixelInternalAPI.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsManagers.Loaders
{
    internal class RoomFunctionLoader 
    {
        public static void LoadRoomFunctions()
        {

            //Gotta Sweep Room CopyCast Hall Textures
            foreach (WeightedRoomAsset closet in GenericExtensions.FindResourceObject<GottaSweep>().potentialRoomAssets)
            {
                var sweepCloset = closet.selection;
                var closetContainer = sweepCloset.roomFunctionContainer = new GameObject("ClosetRoomFunction").AddComponent<RoomFunctionContainer>();
                closetContainer.ReflectionSetVariable("functions", new List<RoomFunction>());
                closetContainer.gameObject.ConvertToPrefab(true);
                var copyCastFunction = new GameObject("CopycastFunction").AddComponent<CopycastRoomFunction>();
                copyCastFunction.gameObject.ConvertToPrefab(true);
                closetContainer.AddFunction(copyCastFunction);
            }

            //PitStop Halls Textures Each Floor
            var pitStopScene = Resources.FindObjectsOfTypeAll<SceneObject>().First(x => x.levelTitle == "PIT");
            var pitStopConainer = pitStopScene.levelAsset.rooms[0].roomFunctionContainer = new GameObject("PitStopHallFunction").AddComponent<RoomFunctionContainer>();
            pitStopConainer.ReflectionSetVariable("functions", new List<RoomFunction>());
            pitStopConainer.gameObject.ConvertToPrefab(true);
            var pitStopPlusFunction = new GameObject("CopycastFunction").AddComponent<PitStopPlusRoomFunction>();
            pitStopPlusFunction.textures.Add("F1", [AssetsHelper.Get<Texture2D>("PinkWallBrick"), AssetsHelper.Get<Texture2D>("GenericWoodFloor"), AssetsHelper.Get<Texture2D>("GenericCeiling1")]);
            pitStopPlusFunction.textures.Add("F2", [AssetsHelper.Get<Texture2D>("RodapéWall"), AssetsHelper.Get<Texture2D>("Carpet_UpRed"), AssetsHelper.Get<Texture2D>("HapracoNewCeiling")]);
            pitStopPlusFunction.textures.Add("F3", [AssetsHelper.Get<Texture2D>("PinkSaloonWall"), AssetsHelper.Get<Texture2D>("WhiytellyRedCarpet"), AssetsHelper.Get<Texture2D>("FancyCeiling")]);

            pitStopPlusFunction.gameObject.ConvertToPrefab(true);
            pitStopConainer.AddFunction(pitStopPlusFunction);

            //Principal Outline
            foreach (RoomAsset office in Resources.FindObjectsOfTypeAll<RoomAsset>().Where(x => x.roomFunctionContainer != null))
            {
                office.offLimits = true;
                var copyCastFunction = office.roomFunctionContainer.gameObject.AddComponent<OutilineRoomFunction>();
                copyCastFunction.active = false;
                office.roomFunctionContainer.AddFunction(copyCastFunction);
            }
        }
    }
}
