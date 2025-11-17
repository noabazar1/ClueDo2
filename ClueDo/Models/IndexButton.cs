using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Models
{
    public class IndexButton : Button
    {
        public int RowIndex { get; set; } 
        public int ColumnIndex { get; set; } 
        public IndexButton(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            HeightRequest = 25;
            WidthRequest = HeightRequest;
            BackgroundColor = Color.FromArgb("#F7D275");
            BorderColor = Colors.Black;
            BorderWidth = 0.5;
            CornerRadius = 0;
        }
    }
}
