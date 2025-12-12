using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Models
{
    public class GameBoard
    {
        public int Rows { get; set; } = 15;
        public int Columns { get; set; } = 15;
        public PlayerPiece PlayerPiece { get; set; } = new PlayerPiece();
    }
}
