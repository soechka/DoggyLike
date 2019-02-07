using DogLeave.Core;
using DogLeave.Systems;
using RLNET;
using System;

namespace Prjct707 {

    class Engine {
        // The screen width and height
        public static readonly int _screenWidth = 100;
        public static readonly int _screenHeight = 70;
        private static RLRootConsole _rootConsole;

        // Map console width and height
        private static readonly int _mapWidth = 60;
        private static readonly int _mapHeight = 70;
        private static RLConsole _mapConsole;

        // Message pool
        private static readonly int _messageWidth = 20;
        private static readonly int _messageHeight = 70;
        private static RLConsole _messageConsole;

        // Stats pool
        private static readonly int _statsWidth = 20;
        private static readonly int _statsHeight = 40;
        private static RLConsole _statsConsole;

        // Inventory pool
        private static readonly int _invWidth = 20;
        private static readonly int _invHeight = 30;
        private static RLConsole _invConsole;

        private static bool _renderRequired = true;

        static void Main(string[] args) {
            string fontFileName = "terminal8x8.png";
            string consoleTitle = "p.707";
            _rootConsole = new RLRootConsole(fontFileName, _screenWidth, _screenHeight, 8, 8, 1f, consoleTitle);
            _mapConsole = new RLConsole(_mapWidth, _mapHeight);
            _messageConsole = new RLConsole(_messageWidth, _mapHeight);
            _statsConsole = new RLConsole(_statsWidth, _mapHeight);
            _invConsole = new RLConsole(_invWidth, _invHeight);
            Player = new Player();
            CommandSystem = new CommandSystem();
            GeneratorForMaps generatorForMaps = new GeneratorForMaps(_mapWidth, _mapHeight);
            DungeonMap = generatorForMaps.CreateMap();
            DungeonMap.UpdatePlayerFoV();
            _rootConsole.Update += OnRootConsoleUpdate;
            _rootConsole.Render += OnRootConsoleRender;

            _mapConsole.SetBackColor(0, 0, _mapWidth, _mapHeight, Colors.FloorBackground);
            _mapConsole.Print(1, 1, "Map", Colors.TextHeading);
            _messageConsole.SetBackColor(0, 0, _messageWidth, _messageHeight, Swatch.PrimaryDarkest);
            _messageConsole.Print(1, 1, "Messages", Colors.TextHeading);
            _statsConsole.SetBackColor(0, 0, _statsWidth, _statsHeight, Swatch.PrimarySandy);
            _statsConsole.Print(1, 1, "Stats", Colors.TextHeading);
            _invConsole.SetBackColor(0, 0, _invWidth, _invHeight, Swatch.PrimaryDarker);
            _invConsole.Print(1, 1, "Inv", Colors.TextHeading);

            //
            _rootConsole.Run();
        }

        private static void OnRootConsoleRender(object sender, UpdateEventArgs e) {
            if (_renderRequired) {
                DungeonMap.Draw(_mapConsole);
                Player.Draw(_mapConsole, DungeonMap);
                RLConsole.Blit(_mapConsole, 0, 0, _mapWidth, _mapHeight, _rootConsole, _messageWidth, 0);
                RLConsole.Blit(_messageConsole, 0, 0, _messageWidth, _messageHeight, _rootConsole, 0, 0);
                RLConsole.Blit(_statsConsole, 0, 0, _statsWidth, _statsHeight, _rootConsole, _mapWidth + _messageWidth, 0);
                RLConsole.Blit(_invConsole, 0, 0, _invWidth, _invHeight, _rootConsole, _mapWidth + _messageWidth, _statsHeight);
                // RLNET draw setted console
                _rootConsole.Draw();

                _renderRequired = false;
            }
           // throw new NotImplementedException();
        }

        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e) {
            bool didPlayerAct = false;
            RLKeyPress keyPress = _rootConsole.Keyboard.GetKeyPress();

            if (keyPress != null) {
                if (keyPress.Key == RLKey.Up) {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Up);
                }
                else if (keyPress.Key == RLKey.Down) {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Down);
                }
                else if (keyPress.Key == RLKey.Left) {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Left);
                }
                else if (keyPress.Key == RLKey.Right) {
                    didPlayerAct = CommandSystem.MovePlayer(Direction.Right);
                }
                else if (keyPress.Key == RLKey.Escape) {
                    _rootConsole.Close();
                }
            }

            if (didPlayerAct) {
                _renderRequired = true;
            }
            //  throw new NotImplementedException();
        }

        public static Player Player { get; private set; }
        public static DungeonMap DungeonMap { get; private set; }
        public static CommandSystem CommandSystem { get; private set; }
    }
}
