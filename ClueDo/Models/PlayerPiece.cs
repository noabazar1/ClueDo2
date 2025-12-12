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
        public IndexButton CurrentButton { get; set; } = new IndexButton(0,0);
    }
}
