using Plugin.CloudFirestore.Attributes;
using ClueDo.Models;
using System.Runtime.Serialization;

namespace ClueDo.Models
{
    public abstract class PlayerModel
    {
        public Position[] startPositions = [new(9, 14), new(4, 14), new(0, 10), new(0, 4), new(10, 0)];
        protected Color[] playerColors = [Colors.Magenta, Colors.Green, Colors.Orange, Colors.Beige, Colors.Red];
        public int Index { get; set; }
        public string Name { get; set; }
        [Ignored]
        public IndexButton Button { get; set; }
        public Position Position { get; set; }
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
