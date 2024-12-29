using MTM101BaldAPI.Reflection;
using nbbpfei_reworked.FundamentalsPlayerData;
using System.Linq;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsContent.RoomFunctions
{
    public class MestizoFunction : RoomFunction
    {
        public override void Initialize(RoomController room)
        {
            base.Initialize(room);

            try
            {
                var builder = FindObjectOfType<LevelBuilder>();

                if (!PlayerDataLoader.GetPlayer().mestizoColors)
                    return;

                if (builder.scene.levelObject == null)
                    return;

                if (builder.scene.levelObject.roomGroup.FirstOrDefault(x => x.potentialRooms[0].selection.category == room.category) == null)
                    return;

                var wallTextures = builder.scene.levelObject.roomGroup.FirstOrDefault(x => x.potentialRooms[0].selection.category == room.category).wallTexture;
                var floorTextures = builder.scene.levelObject.roomGroup.FirstOrDefault(x => x.potentialRooms[0].selection.category == room.category).floorTexture;
                var ceilingTextures = builder.scene.levelObject.roomGroup.FirstOrDefault(x => x.potentialRooms[0].selection.category == room.category).ceilingTexture;

                if (wallTextures.Length == 0 || floorTextures.Length == 0 || ceilingTextures.Length == 0)
                    return;

                int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

                var wallTex = WeightedSelection<Texture2D>.ControlledRandomSelection(wallTextures, new(seed));
                var florTex = WeightedSelection<Texture2D>.ControlledRandomSelection(floorTextures, new(seed));
                var ceilTex = WeightedSelection<Texture2D>.ControlledRandomSelection(ceilingTextures, new(seed));

                room.wallTex = wallTex;
                room.florTex = florTex;
                room.ceilTex = ceilTex;
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
            catch { }
        }
    }
}
