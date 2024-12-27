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
                var closetContainer = sweepCloset.roomFunctionContainer;

                if (sweepCloset.roomFunctionContainer == null)
                {
                    sweepCloset.roomFunctionContainer = new GameObject("ClosetRoomFunction").AddComponent<RoomFunctionContainer>();
                    closetContainer = sweepCloset.roomFunctionContainer;
                    closetContainer.ReflectionSetVariable("functions", new List<RoomFunction>());
                    closetContainer.gameObject.ConvertToPrefab(true);
                }

                var copyCastFunction = closetContainer.gameObject.AddComponent<CopycastRoomFunction>();
                copyCastFunction.gameObject.ConvertToPrefab(true);
                closetContainer.AddFunction(copyCastFunction);
            }

            //PitStop Halls Textures Each Floor
            var pitStopScene = Resources.FindObjectsOfTypeAll<SceneObject>().First(x => x.levelTitle == "PIT");
            var pitStopConainer = pitStopScene.levelAsset.rooms[0].roomFunctionContainer = new GameObject("PitStopHallFunction").AddComponent<RoomFunctionContainer>();
            pitStopConainer.ReflectionSetVariable("functions", new List<RoomFunction>());
            pitStopConainer.gameObject.ConvertToPrefab(true);
            var pitStopPlusFunction = pitStopConainer.gameObject.AddComponent<PitStopPlusRoomFunction>();
            pitStopConainer.AddFunction(pitStopPlusFunction);

            //Principal Outline
            foreach (RoomAsset room in Resources.FindObjectsOfTypeAll<RoomAsset>().Where(x => x.roomFunctionContainer != null))
            {
                room.offLimits = true;

                if (room.roomFunctionContainer.gameObject.GetComponent<OutilineRoomFunction>() == null)
                {
                    var copyCastFunction = room.roomFunctionContainer.gameObject.AddComponent<OutilineRoomFunction>();
                    room.roomFunctionContainer.AddFunction(copyCastFunction);
                }

                if (room.roomFunctionContainer.gameObject.GetComponent<MestizoFunction>() == null)
                {
                    var mestizoFunction = room.roomFunctionContainer.gameObject.AddComponent<MestizoFunction>();
                    room.roomFunctionContainer.AddFunction(mestizoFunction);
                }
            }

            foreach (RoomAsset classroom in Resources.FindObjectsOfTypeAll<RoomAsset>().Where(x => x.roomFunctionContainer != null))
            {
                if (!classroom.roomFunctionContainer.gameObject.GetComponent<CustomWoodFunction>())
                {
                    var woodFunction = classroom.roomFunctionContainer.gameObject.AddComponent<CustomWoodFunction>();
                    classroom.roomFunctionContainer.AddFunction(woodFunction);
                }
            }
        }
    }
}
