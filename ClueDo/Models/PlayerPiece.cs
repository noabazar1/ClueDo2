using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Models
{
    public class PlayerPiece
    {
        public string Name { get; set; } = string.Empty;
        public Color Color { get; set; } = Colors.Transparent;
        public Position CurrentPosition { get; set; } = new Position(0, 0);
        public PlayerPiece(string Name, Color color, Position position)
        {
            this.Name = Name;
            this.Color = color;
            CurrentPosition = position;
        }
        public PlayerPiece()
        {
        }
    }
}
