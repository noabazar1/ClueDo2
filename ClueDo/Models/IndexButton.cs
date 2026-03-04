using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Models
{
    public class IndexButton : Button
    {
        private readonly Color baseColor = Colors.Transparent;
        public int Row { get; set; }
        public int Column { get; set; }
        public int GridIndex { get; set; }
        public bool IsDoor { get; set; }
        public string? RoomName { get; set; }
        public IndexButton(int row, int columnIndex)
        {
            Row = row;
            Column = columnIndex;
            BackgroundColor = baseColor;
            BorderWidth = 0.5;
            BorderColor = Colors.Black;
            Margin = 0;
            Padding = 0;
            CornerRadius = 0;
        }
        public IndexButton()
        {
            Row = 0;
            Column = 0;
            BackgroundColor = baseColor;
            BorderWidth = 0.5;
            BorderColor = Colors.Black;
            Margin = 0;
            Padding = 0;
            CornerRadius = 0;
        }
        public void RestoreColor()
        {
            BackgroundColor = baseColor;
        }
    }
}