namespace ClueDo.Models
{
    /// <summary>
    /// class that represents a position in the grid of the game, which holds the row and column index of 
    /// the position. The Position class is used to represent the position of the player in the grid, and
    /// to determine the player's movement and interactions with the grid. With primary constructor.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public class Position(int row, int column)
    {
        public int Row { get; set; } = row;
        public int Column { get; set; } = column;
        /// <summary>
        /// constructor for the Position class, which initializes the properties of the position, such as the row
        /// and column index.
        /// </summary>
        public Position() : this(0, 0) { }
    }
}
