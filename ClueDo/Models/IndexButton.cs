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
        public int ColumnIndx { get; set; } 
        public IndexButton(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndx = columnIndex;
            HeightRequest = 25;
            WidthRequest = HeightRequest;
            BackgroundColor = Color.FromArgb("#F7D275");
            BorderColor = Colors.Black;
            CornerRadius = 0;
        }
    }
}
