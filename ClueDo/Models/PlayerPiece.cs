using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Models
{
    public class PlayerPiece
    {
        public Player Player { get; }
        public Color Color { get; }

        public PlayerPiece(Player player, Color color)
        {
            Player = player;
            Color = color;
        }
    }
}
