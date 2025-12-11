using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Models
{
    public class Player : Button
    {
        public string Name { get; set; } = string.Empty;
        public Color Color { get; set; } = Color.FromArgb("#B0251A");
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public int GridIndex { get; set; }

        internal void SetStartByColor()
        {
            if (Color == Color.FromArgb(Keys.Red))
            {
                StartRow = 10;
                StartColumn = 0;
            }
            if (Color == Color.FromArgb(Keys.Green))
            {
                StartRow = 5;
                StartColumn = 14;
            }
            if (Color == Color.FromArgb(Keys.Blue))
            {
                StartRow = 0;
                StartColumn = 10;
            }
            if (Color == Color.FromArgb(Keys.Plum))
            {
                StartRow = 0;
                StartColumn = 4;
            }
            if (Color == Color.FromArgb(Keys.Mustard))
            {
                StartRow = 4;
                StartColumn = 0;
            }
            if (Color == Colors.White)
            {
                StartRow = 9;
                StartColumn = 14;
            }
        }
    }
}
