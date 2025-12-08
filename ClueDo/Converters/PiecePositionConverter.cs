using ClueDo.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Converters
{
    public class PiecePositionConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value is GamePiece piece)
            {
                return new Rect(piece.X * 30, piece.Y * 30, 20, 20);
            }
            return new Rect(0, 0, 20, 20);
        }
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
