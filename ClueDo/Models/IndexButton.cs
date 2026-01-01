using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Models
{
    public class IndexButton : Button
    {
        public int Row { get; set; } 
        public int Column { get; set; }
        public int GridIndex { get; set; }
        public IndexButton(int row, int columnIndex)
        {
            Row = row;
            Column = columnIndex;
            HeightRequest = 25;
            WidthRequest = HeightRequest;
            BackgroundColor = Color.FromArgb("#F7D275");
            BorderColor = Colors.Black;
            BorderWidth = 0.5;
            CornerRadius = 0;
        }
        public IndexButton()
        {
            Row = 0;
            Column = 0;
            HeightRequest = 25;
            WidthRequest = HeightRequest;
            BackgroundColor = Color.FromArgb("#F7D275");
            BorderColor = Colors.Black;
            BorderWidth = 0.5;
            CornerRadius = 0;
        }
    }
}
