using Plugin.CloudFirestore.Attributes;

namespace ClueDo.Models
{
    /// <summary>
    /// class that holds the data of the player, such as the name, the index, the position, the moves left,
    /// the dice value, whether the player is in a room or not, and whether the player is eliminated or not.
    /// This class is used in the GamePage to display the player's information, and to update the player's 
    /// position and status during the game.
    /// </summary>
    public abstract class PlayerModel
    {
        public Position[] startPositions = [new(10, 14), new(4, 14), new(0, 10), new(0, 4)];
        protected Color[] playerColors = [Colors.Red, Colors.Green, Colors.Orange, Colors.Beige];
        public int Index { get; set; }
        public string Name { get; set; }
        [Ignored]
        public IndexButton Button { get; set; }
        public Position Position { get; set; }
        public int MovesLeft { get; set; }
        public int DiceValue { get; set; }
        public bool IsInRoom { get; set; }
        public bool IsEliminated { get; set; }
        [Ignored]
        public Color Color => playerColors[Index];
        /// <summary>
        /// constructor for the PlayerModel class, which initializes the properties of the player. The 
        /// constructor takes three parameters, which are the name of the player, the index of the player, 
        /// and the button that represents the player in the grid of the game. The position of the player 
        /// is set to the corresponding start position based on the index of the player. This constructor
        /// is used when creating a PlayerModel object with specified name, index, and button.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <param name="button"></param>
        public PlayerModel(string name, int index, IndexButton button)
        {
            Name = name;
            Index = index;
            Button = button;
            Position = startPositions[index];
        }
    }
}
