namespace ClueDo.Models
{
    /// <summary>
    /// class that represents a button in the grid of the game, which holds the row and column index of 
    /// the button, the grid index of the button, whether the button represents a door or not, and the 
    /// name of the room that the button represents (if it is a door). The IndexButton class inherits from
    /// the Button class, and it is used to create the grid of the game, where each button represents a 
    /// cell in the grid. The properties of the IndexButton class are used to determine the position of 
    /// the button in the grid, and to determine whether the button represents a door or not. 
    /// </summary>
    public partial class IndexButton : Button
    {
        private readonly Color baseColor = Colors.Transparent;
        public int Row { get; set; }
        public int Column { get; set; }
        public int GridIndex { get; set; }
        public bool IsDoor { get; set; }
        public string? RoomName { get; set; }
        /// <summary>
        /// constructor for the IndexButton class, which initializes the properties of the button, such as
        /// the row and column index, the background color, the border width and color, the margin and 
        /// padding, and the corner radius. The constructor takes two parameters, which are the row and 
        /// column index of the button in the grid. The background color is set to transparent by default,
        /// and the border is set to a thin black line. The margin and padding are set to zero, and the 
        /// corner radius is set to zero to create a square button. This constructor is used when creating 
        /// an IndexButton object with specified row and column indices.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex"></param>
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
        /// <summary>
        /// constructor for the IndexButton class, which initializes the properties of the button, such as
        /// the row and column index, the background color, the border width and color, the margin and
        /// padding, and the corner radius.  The background color is set to transparent by default,
        /// and the border is set to a thin black line. The margin and padding are set to zero, and the
        /// corner radius is set to zero to create a square button. This constructor is used when creating
        /// an IndexButton object with no specified row and column indices.
        /// </summary>
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
        /// <summary>
        /// method to restore the background color of the button to the base color, which is transparent. 
        /// </summary>
        public void RestoreColor()
        {
            BackgroundColor = baseColor;
        }
    }
}