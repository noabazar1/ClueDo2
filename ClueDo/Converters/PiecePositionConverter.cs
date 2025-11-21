using ClueDo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.Converters
{
    public class PiecePositionConverter
    {
        public object? Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var piece = value as GamePiece;
            if (piece == null)
                return null;
            double cellSize = 30;
            return new Rect(piece.X * cellSize, piece.Y * cellSize, 20, 20);
        }
    }
}
