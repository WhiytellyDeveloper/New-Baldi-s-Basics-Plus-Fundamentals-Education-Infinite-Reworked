using MTM101BaldAPI.Reflection;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsContent.RoomFunctions
{
    public class CopycastRoomFunction : RoomFunction
    {
        public override void Initialize(RoomController room)
        {
            base.Initialize(room);

            var _cell = room.ec.FindHallways()[0][0];

            if (_cell.room.category == RoomCategory.Hall)
            {
                var wallTex = _cell.room.wallTex;
                var florTex = _cell.room.florTex;
                var ceilTex = _cell.room.ceilTex;

                room.wallTex = wallTex;
                room.florTex = florTex;
                room.ceilTex = ceilTex;

                if (room.wallTex == wallTex && room.florTex == florTex && room.ceilTex == ceilTex)
                {
                    texturesLoaded = true;
                    room.GenerateTextureAtlas();
                }
            }
            else
            {
                Debug.LogError($"An error was made in the CopycastRoomFunction function, the room textures from {Room.name} were not copy correctly and were not applied, I ask you to urgently report this bug, please :)");
            }


            if (texturesLoaded)
            {
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

                    foreach (Window window in FindObjectsOfType<Window>())
                        window.UpdateTextures();
                }
            }
            else
                Debug.LogError($"An error was made in the CopycastRoomFunction function, the room textures from {Room.name} were not copy correctly and were not applied, I ask you to urgently report this bug, please :) - after texture loader");
        }

        protected bool texturesLoaded;
    }
}
