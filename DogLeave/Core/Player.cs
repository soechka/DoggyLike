using System;

namespace DogLeave.Core {
    public class Player : Actor {
        public Player() {
            Awareness = 15;
            Name = "Doggy";
            Color = Colors.Player;
            Symbol = '@';
            X = 10;
            Y = 10;
        }
    }
}
