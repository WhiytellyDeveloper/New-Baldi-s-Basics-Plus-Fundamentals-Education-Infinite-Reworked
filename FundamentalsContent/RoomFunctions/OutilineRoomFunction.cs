using MTM101BaldAPI.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace nbbpfei_reworked.FundamentalsContent.RoomFunctions
{
    public class OutilineRoomFunction : RoomFunction
    {
        protected List<Cell> endCells = new List<Cell>();
        public bool preMadeMap;
        public bool active = true;

        public override void Build(LevelBuilder level, System.Random random)
        {
            base.Build(level, random);

            if (level as LevelLoader)
            {
                Debug.LogError("The function can ONLY be applied in rooms in the random floor generator");
                preMadeMap = true;
                return;
            }

            if (!active)
                return;

            foreach (Cell cell in room.cells)
            {
                foreach (Direction dir in Directions.All())
                {
                    var pos = cell.position + dir.ToIntVector2();
                    var targetCell = room.ec.CellFromPosition(pos);

                    if (targetCell.Null)
                    {
                        room.ec.CreateCell(0, pos, room.ec.mainHall);

                        foreach (Direction closeDir in Directions.All())
                            room.ec.CloseCell(pos, closeDir);

                        foreach (Direction nullDir in Directions.All())
                        {
                            var adjacentPos = pos + nullDir.ToIntVector2();
                            var adjacentCell = room.ec.CellFromPosition(adjacentPos);

                            if (adjacentCell != null && adjacentCell.room.category == RoomCategory.Hall)
                            {
                                room.ec.ConnectCells(pos, nullDir);
                                room.ec.UpdateCell(pos);

                                if (room.ec.CellFromPosition(pos).shape == TileShapeMask.End)
                                    endCells.Add(room.ec.CellFromPosition(pos));
                            }
                        }
                    }
                }
            }

            foreach (Cell endCell in endCells)
            {
                foreach (Direction dir in Directions.All())
                {
                    var pos = endCell.position + dir.ToIntVector2();
                    var targetCell = room.ec.CellFromPosition(pos);

                    if (targetCell.Null && !targetCell.offLimits)
                    {
                        room.ec.CreateCell(0, pos, room.ec.mainHall);

                        foreach (Direction closeDir in Directions.All())
                            room.ec.CloseCell(pos, closeDir);

                        foreach (Direction nullDir in Directions.All())
                        {
                            var adjacentPos = pos + nullDir.ToIntVector2();
                            var adjacentCell = room.ec.CellFromPosition(adjacentPos);

                            if (adjacentCell != null && adjacentCell.room.category == RoomCategory.Hall)
                            {
                                room.ec.ConnectCells(pos, nullDir);
                                room.ec.UpdateCell(pos);
                            }
                        }
                    }
                }
            }

            foreach (Cell endCell in room.ec.cells)
            {
                if (endCell.shape == TileShapeMask.End || endCell.shape == TileShapeMask.Closed)
                {

                    foreach (Direction dir in Directions.All())
                    {
                        var pos = endCell.position + dir.ToIntVector2();
                        var targetCell = room.ec.CellFromPosition(pos);

                        if (targetCell.Null)
                        {
                            room.ec.CreateCell(0, pos, room.ec.mainHall);

                            foreach (Direction closeDir in Directions.All())
                                room.ec.CloseCell(pos, closeDir);

                            bool makeChanges = false;

                            foreach (Direction nullDir in Directions.All())
                            {
                                var adjacentPos = pos + nullDir.ToIntVector2();
                                var adjacentCell = room.ec.CellFromPosition(adjacentPos);

                                if (adjacentCell != null && adjacentCell.room.category == RoomCategory.Hall)
                                {
                                    room.ec.ConnectCells(pos, nullDir);
                                    room.ec.UpdateCell(pos);
                                    makeChanges = true;
                                }
                            }

                            if (!makeChanges)
                                room.ec.DestroyCell(room.ec.CellFromPosition(pos));
                        }
                    }
                }
            }
        }

        public override void OnGenerationFinished()
        {
            base.OnGenerationFinished();

            if (!active)
                return;

            var scene = GameObject.FindObjectOfType<GameInitializer>().ReflectionGetVariable("sceneObject") as SceneObject;

            if (scene.levelObject == null)
                return;

            foreach (Elevator elv in room.ec.elevators)
            {
                var door = elv.Door;

                foreach (Direction dir in Directions.All())
                {
                    var pos = room.ec.CellFromPosition(elv.Door.transform.position).position + dir.ToIntVector2();
                    var cell = room.ec.CellFromPosition(pos);
                    room.ec.CloseCell(pos, dir.GetOpposite());
                }

                var clocks = elv.ReflectionGetVariable("clock") as DigitalClock[];

                foreach (DigitalClock clock in clocks)
                {
                    var clockPos = room.ec.CellFromPosition(clock.transform.position);
                    room.ec.SwapCell(clockPos.position, clockPos.room, 0);

                    foreach (Direction closeDir in Directions.All())
                        room.ec.CloseCell(clockPos.position, closeDir);

                    foreach (Direction openDir in Directions.All())
                    {
                        var pos = clockPos.position + openDir.ToIntVector2();

                        if (room.ec.CellFromPosition(pos).room.category == RoomCategory.Hall)
                        room.ec.ConnectCells(clockPos.position, openDir);
                    }
                }

                var frontPos = room.ec.CellFromPosition(elv.Door.position);
                room.ec.SwapCell(frontPos.position, frontPos.room, 0);
            }
        }
    }
}
