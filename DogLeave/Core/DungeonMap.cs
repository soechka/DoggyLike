using Prjct707;
using RLNET;
using RogueSharp;
using System;

namespace DogLeave.Core {
    public class DungeonMap : Map {
        public void Draw(RLConsole mapConsole) {
            mapConsole.Clear();
            foreach( Cell cell in GetAllCells()) {
                SetConsoleSymbolForSell(mapConsole, cell);
            }
        }

        public void SetConsoleSymbolForSell(RLConsole console, Cell cell) {
            if (!cell.IsExplored) {
                return;
            }
            if (IsInFov (cell.X, cell.Y)) {
                if (cell.IsWalkable) {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
                } else {
                    console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '#');
                }
            } else {
                if (cell.IsWalkable) {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackgroundFov, '.');
                } else {
                    console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackgroundFov, '#');
                }
            }
        } 

        // Method will be called any time we moved player to update FoV
        public void UpdatePlayerFoV() {
            Player player = Engine.Player;
            ComputeFov(player.X, player.Y, player.Awareness, true);
            foreach (Cell cell in GetAllCells()) {
                if (IsInFov(cell.X, cell.Y)) {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        // Returns true when able to place the Actor on the cell or false otherwise
        public bool SetActorPosition(Actor actor, int x, int y) {
            if (GetCell(x, y).IsWalkable) {
                SetIsWalkable(actor.X, actor.Y, true);
                actor.X = x;
                actor.Y = y;
                SetIsWalkable(actor.X, actor.Y, false);
                if (actor is Player) {
                    UpdatePlayerFoV();
                }
                return true;
            }
            return false;
        }

        public void SetIsWalkable(int x, int y, bool isWalkable) {
            ICell cell = GetCell(x, y);
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
        }
    }
}
