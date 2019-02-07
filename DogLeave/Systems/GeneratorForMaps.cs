using RogueSharp;
using RLNET;
using System;
using DogLeave.Core;

namespace DogLeave.Systems {
    public class GeneratorForMaps {
        private readonly int _width;
        private readonly int _height;

        private readonly DungeonMap _map;

        public GeneratorForMaps(int width, int heigth) {
            _width = width;
            _height = heigth;
            _map = new DungeonMap();
        }

        public DungeonMap CreateMap() {
            _map.Initialize(_width, _height);

            foreach (Cell cell in _map.GetAllCells()) {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }
            foreach (Cell cell in _map.GetCellsInRows(0, _height-1)) {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }
            foreach (Cell cell in _map.GetCellsInColumns(0, _width - 1)) {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            return _map;
        }
    }
}
