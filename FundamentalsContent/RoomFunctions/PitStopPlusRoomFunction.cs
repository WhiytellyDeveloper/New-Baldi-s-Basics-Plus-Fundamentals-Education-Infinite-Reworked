using MTM101BaldAPI.Reflection;
using nbbpfei_reworked.FundamentalsManagers;
using System.Collections.Generic;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsContent.RoomFunctions
{
    public class PitStopPlusRoomFunction : RoomFunction
    {
        public override void Initialize(RoomController room)
        {
            base.Initialize(room);

            if (Singleton<CoreGameManager>.Instance.nextLevel == null)
                return;

            string floor = Singleton<CoreGameManager>.Instance.nextLevel.levelTitle;

            if (!textures.ContainsKey(floor))
                return;

            room.wallTex = textures[floor][0];
            room.florTex = textures[floor][1];
            room.ceilTex = textures[floor][2];

            room.GenerateTextureAtlas();

            foreach (Cell cell in room.cells)
                cell.SetBase(room.baseMat);

            foreach (Door door in room.doors)
            {
                if (door is StandardDoor standardDoor)
                {
                    standardDoor.ReflectionSetVariable("bg", new Texture2D[] { door.aTile.room.wallTex, door.bTile.room.wallTex });
                    standardDoor.UpdateTextures();
                }
                else if (door is SwingDoor swingDoor)
                {
                    swingDoor.ReflectionSetVariable("bg", new Texture2D[] { door.aTile.room.wallTex, door.bTile.room.wallTex });
                    swingDoor.UpdateTextures();
                }
            }

            foreach (Window window in FindObjectsOfType<Window>())
                window.UpdateTextures();
        }

        public Dictionary<string, Texture2D[]> textures = new Dictionary<string, Texture2D[]> {
             {"F1", [AssetsHelper.Get<Texture2D>("PinkWallBrick"), AssetsHelper.Get<Texture2D>("GenericWoodFloor"), AssetsHelper.Get<Texture2D>("GenericCeiling1")]},
             {"F2", [AssetsHelper.Get<Texture2D>("RodapéWall"), AssetsHelper.Get<Texture2D>("Carpet_UpRed"), AssetsHelper.Get<Texture2D>("HapracoNewCeiling")]},
             {"F3", [AssetsHelper.Get<Texture2D>("PinkSaloonWall"), AssetsHelper.Get<Texture2D>("WhiytellyRedCarpet"), AssetsHelper.Get<Texture2D>("FancyCeiling")]}
        };
    }
}
