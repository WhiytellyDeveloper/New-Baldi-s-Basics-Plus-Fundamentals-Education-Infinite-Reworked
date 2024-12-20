using MTM101BaldAPI.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
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

            /*
            foreach (Window window in FindObjectsOfType<Window>())
                window.UpdateTextures();
            */
        }

        public Dictionary<string, Texture2D[]> textures = [];
    }
}
