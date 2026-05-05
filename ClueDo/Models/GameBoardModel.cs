namespace ClueDo.Models
{
    /// <summary>
    /// class representing the game board, it contains methods for building the board, creating the grid and 
    /// buttons, blocking areas, building rooms, making doors, resetting and restoring colors, updating
    /// buttons, checking if a position is blocked and handling turns. It is an abstract class that will be
    /// implemented by the GameBoard class in the ModelsLogic folder.
    /// </summary>
    public abstract class GameBoardModel
    {
        public const int BoardSize = 15;
        public bool roomsBuilt = false;
        public bool isBuilt = false;
        /// <summary>
        /// abstract method for building the game board, it takes a Grid and an EventHandler as parameters.
        /// The Grid is the container for the buttons and the EventHandler is used to handle the click events
        /// of the buttons. It will call the CreateGrid and CreateButtons methods to create the grid and
        /// buttons, and then call the BuildRooms method to build the rooms on the board. It will also set
        /// the isBuilt variable to true once the board is built.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="clickHandler"></param>
        public abstract void Build(Grid board, EventHandler clickHandler);
        /// <summary>
        /// abstract method for creating the grid, it takes a Grid as a parameter and creates a 15x15 
        /// grid on it.
        /// </summary>
        /// <param name="board"></param>
        public abstract void CreateGrid(Grid board);
        /// <summary>
        /// abstract method for creating the buttons, it takes a Grid and an EventHandler as parameters.
        /// The Grid is the container for the buttons and the EventHandler is used to handle the click events
        /// of the buttons. It will create a button for each cell in the grid and add it to the grid. 
        /// It will also set the click event handler for each button to the provided EventHandler. 
        /// Each button will have its own click event handler that will be used to handle the player's
        /// interactions with the game board, such as moving their piece or making a suggestion.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="clickHandler"></param>
        public abstract void CreateButtons(Grid board, EventHandler clickHandler);
        /// <summary>
        /// abstract method for blocking an area on the board.
        /// </summary>
        /// <param name="rowStart"></param>
        /// <param name="rowEnd"></param>
        /// <param name="colStart"></param>
        /// <param name="colEnd"></param>
        public abstract void BlockArea(int rowStart, int rowEnd, int colStart, int colEnd);
        /// <summary>
        /// abstract method for building the rooms on the board, it will be called after the grid and
        /// buttons are created. It will set the roomsBuilt variable to true once the rooms are built.
        /// The method will define the layout of the rooms on the board, including the positions of the
        /// walls and doors. It will also set the properties of the buttons that are part of the rooms,
        /// such as their background color and whether they are blocked or not. The method will also
        /// ensure that the rooms are built according to the standard Clue board layout, with the correct
        /// placement of the rooms and doors.
        /// </summary>
        public abstract void BuildRooms();
        /// <summary>
        /// abstract method for making a door on the board, it takes the row and column of the button that
        /// will be the door, and the name of the room that the door will lead to. The method will set the
        /// properties of the button at the specified row and column to indicate that it is a door, such
        /// as changing its background color and setting its RoomName property to the provided room name.
        /// The method will be called during the BuildRooms method to create the doors for each room on
        /// the board.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="roomName"></param>
        public abstract void MakeDoor(int row, int col, string roomName);
        /// <summary>
        /// abstract method for resetting the colors of the buttons on the board, it will be called at the
        /// end of each turn to reset the colors of the buttons to their default state. The method will
        /// iterate through all the buttons on the board and set their background color to the default 
        /// color. 
        /// </summary>
        public abstract void ResetBoardColors();
        /// <summary>
        /// abstract method for restoring the colors of the buttons on the board.
        /// </summary>
        public abstract void RestoreColors();
        /// <summary>
        /// abstract method for updating the color of a button on the board, it takes a Position and a
        /// Color as parameters. The Position parameter specifies the row and column of the button that
        /// will be updated, and the Color parameter specifies the new background color for the button.
        /// The method will set the background color of the button at the specified position to the
        /// provided color. This method will be used to indicate valid moves for the player, as well as
        /// to show the player's current position on the board. It will also be called during the player's
        /// turn to update the colors of the buttons based on the player's actions, such as moving their
        /// piece.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="color"></param>
        public abstract void UpdateButton(Position pos, Color color);
        /// <summary>
        /// abstract method for getting a button from the board, it takes a Position as a parameter and
        /// returns the button at the specified position. The Position parameter specifies the row and
        /// column of the button that will be retrieved. The method will return the button at the 
        /// specified position, which can then be used to check its properties, such as whether it is a
        /// door or part of a room, or to update its color or other properties. This method will be used
        /// in various parts of the game logic, such as when handling player moves or checking for valid
        /// moves, to access the buttons on the board and interact with them based on the player's actions.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public abstract IndexButton GetButton(Position p);
        /// <summary>
        /// abstract method for checking if a position on the board is blocked, it takes a Position as a
        /// parameter and returns a boolean indicating whether the position is blocked or not. The
        /// Position parameter specifies the row and column of the position that will be checked.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public abstract bool IsBlocked(Position p);
        /// <summary>
        /// abstract method for handling the player's turn, it will be called when it is the player's
        /// turn to make a move. The method will fix the board and prepare it for the player's turn,
        /// such as resetting the colors of the buttons.
        /// </summary>
        public abstract void MyTurn();
    }
}
