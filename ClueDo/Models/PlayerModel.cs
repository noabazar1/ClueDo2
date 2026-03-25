using Plugin.CloudFirestore.Attributes;

namespace ClueDo.Models
{
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
        public PlayerModel(string name, int index, IndexButton button)
        {
            Name = name;
            Index = index;
            Button = button;
            Position = startPositions[index];
        }
    }
}
