using Plugin.CloudFirestore.Attributes;
using ClueDo.Models;

namespace ClueDo.Models
{
    public abstract class PlayerModel
    {
        protected Position[] startPositions = [new(4, 2), new(2, 4), new(0, 2), new(2, 0), new(2, 2)];
        protected Color[] playerColors = [Colors.Magenta, Colors.Green, Colors.Orange, Colors.Beige, Colors.Red];
        public int Index { get; set; }
        public string Name { get; set; }
        public Position Position { get; set; }
        [Ignored]
        public Color Color => playerColors[Index];
        public PlayerModel(string name, int index)
        {
            Name = name;
            Index = index;
            Position = startPositions[index];
        }
    }
}
