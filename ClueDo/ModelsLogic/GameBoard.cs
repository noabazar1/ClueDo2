using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    /// <summary>
    /// class representing the game board, with a 2D array of IndexButtons for the grid and a 2D array of
    /// booleans to track blocked areas.
    /// </summary>
    public class GameBoard : GameBoardModel
    {
        public IndexButton[,] buttons;
        public bool[,] blocked;
        /// <summary>
        /// constructor for the GameBoard class, which initializes the buttons and blocked arrays. The buttons
        /// array is a 2D array of IndexButtons that represents the grid of the game, and the blocked array is a
        /// 2D array of booleans that tracks which areas of the board are blocked. The constructor 
        /// initializes both arrays with the specified size, which is determined by the BoardSize constant. 
        /// </summary>
        public GameBoard()
        {
            buttons = new IndexButton[BoardSize, BoardSize];
            blocked = new bool[BoardSize, BoardSize];
        }
        /// <summary>
        /// method for building the game board, it takes a Grid and an EventHandler as parameters. The Grid
        /// is the container for the buttons and the EventHandler is used to handle the click events of the
        /// buttons. The method first checks if the board has already been built using the isBuilt variable.
        /// If the board has not been built, it sets the isBuilt variable to true and calls the CreateGrid
        /// method to create the grid on the provided Grid object, then calls the CreateButtons method to 
        /// create the buttons on the grid and set their click event handlers to the provided EventHandler.
        /// Finally, it calls the BuildRooms method to build the rooms on the board. 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="clickHandler"></param>
        public override void Build(Grid board, EventHandler clickHandler)
        {
            if (!isBuilt)
            {
                isBuilt = true;
                CreateGrid(board);
                CreateButtons(board, clickHandler);
                BuildRooms();
            }
        }
        /// <summary>
        /// method for creating the grid, it takes a Grid as a parameter and creates a 15x15 grid on it. The
        /// method first clears any existing children, row definitions, and column definitions from the 
        /// provided Grid object. Then, it uses a loop to add RowDefinitions and ColumnDefinitions to the
        /// Grid object, creating a grid with the specified number of rows and columns, which is determined 
        /// by the BoardSize constant. Each row and column is set to have a size of GridLength.Star, which
        /// means that they will take up an equal share of the available space in the Grid. This method is 
        /// called by the Build method to create the grid on the game board before adding the buttons and 
        /// building the rooms. 
        /// </summary>
        /// <param name="board"></param>
        public override void CreateGrid(Grid board)
        {
            board.Children.Clear();
            board.RowDefinitions.Clear();
            board.ColumnDefinitions.Clear();
            for (int i = 0; i < BoardSize; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }
        }
        /// <summary>
        /// method for creating the buttons, it takes a Grid and an EventHandler as parameters. The Grid is
        /// the container for the buttons and the EventHandler is used to handle the click events of the 
        /// buttons. The method uses nested loops to iterate through each row and column of the grid, 
        /// creating an IndexButton for each cell in the grid. The IndexButton is initialized with the 
        /// current row and column indices, and its click event handler is set to the provided EventHandler.
        /// Each button is then added to the buttons array. Finally, each button is added to the provided 
        /// Grid object at the corresponding row and column indices. This method is called by the Build 
        /// method to create the buttons on the game board after creating the grid and before building the
        /// rooms.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="clickHandler"></param>
        public override void CreateButtons(Grid board, EventHandler clickHandler)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    IndexButton button = new(row, col);
                    button.Clicked += clickHandler;
                    buttons[row, col] = button;
                    board.Add(button, col, row);
                }
            }
        }
        /// <summary>
        /// method for blocking an area on the board, it takes the starting and ending row and column
        /// indices as parameters. The method uses nested loops to iterate through the specified range of 
        /// rows and columns, setting the corresponding cells in the blocked array to true, which indicates 
        /// that those areas of the board are blocked.
        /// </summary>
        /// <param name="rowStart"></param>
        /// <param name="rowEnd"></param>
        /// <param name="colStart"></param>
        /// <param name="colEnd"></param>
        public override void BlockArea(int rowStart, int rowEnd, int colStart, int colEnd)
        {
            for (int row = rowStart; row <= rowEnd; row++)
            {
                for (int col = colStart; col <= colEnd; col++)
                {
                    blocked[row, col] = true;
                    buttons[row, col].IsEnabled = false;
                    buttons[row, col].BackgroundColor = Colors.Transparent;
                    buttons[row, col].BorderWidth = 0;
                    buttons[row, col].Padding = 0;
                    buttons[row, col].Margin = 0;
                }
            }
        }
        /// <summary>
        /// method for building the rooms on the board, it defines the layout of the rooms according to the
        /// standard Clue board layout. The method uses the BlockArea method to block the areas of the board
        /// that are not part of the rooms, and the MakeDoor method to create doors for each room at the 
        /// appropriate locations. The method also sets the properties of the buttons that are part of the
        /// rooms, such as their background color and whether they are blocked or not. The method ensures 
        /// that the rooms are built according to the standard Clue board layout, with the correct placement
        /// of the rooms and doors. 
        /// </summary>
        public override void BuildRooms()
        {
            BlockArea(11, 14, 11, 14);
            MakeDoor(11, 11, Strings.Kitchen);
            BlockArea(0, 3, 11, 14);
            MakeDoor(3, 11, Strings.Conservatory);
            BlockArea(0, 4, 0, 2);
            MakeDoor(4, 2, Strings.Study);
            BlockArea(11, 14, 0, 3);
            MakeDoor(11, 3, Strings.Lounge);
            BlockArea(10, 14, 6, 9);
            MakeDoor(10, 7, Strings.DiningRoom);
            BlockArea(5, 9, 11, 14);
            BlockArea(6, 8, 14, 14);
            BlockArea(5, 8, 10, 10);
            MakeDoor(7, 10, Strings.Ballroom);
            BlockArea(0, 3, 8, 9);
            MakeDoor(3, 8, Strings.Hall);
            BlockArea(0, 4, 5, 6);
            MakeDoor(4, 5, Strings.Library);
            BlockArea(6, 9, 0, 4);
            MakeDoor(7, 4, Strings.BilliardRoom);
        }
        /// <summary>
        /// method for creating a door on the board, it takes the row and column indices of the door and 
        /// the name of the room that the door belongs to as parameters. The method retrieves the 
        /// IndexButton at the specified row and column indices from the buttons array, sets its IsEnabled 
        /// property to true, its IsDoor property to true, and its RoomName property to the provided room 
        /// name. This method is called by the BuildRooms method to create doors for each room at the 
        /// appropriate locations on the board. The method ensures that the buttons that represent doors 
        /// are enabled and have the correct properties set, which allows players to interact with the 
        /// doors and move between rooms during the game. 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="roomName"></param>
        public override void MakeDoor(int row, int col, string roomName)
        {
            IndexButton btn = buttons[row, col];
            btn.IsEnabled = true;
            btn.IsDoor = true;
            btn.RoomName = roomName;
        }
        /// <summary>
        /// method for resetting the colors of the buttons on the board, it iterates through each cell in
        /// the grid and checks if the cell is not blocked and if the button at that cell is not null. If 
        /// both conditions are true, it sets the background color of the button to transparent. This 
        /// method is used to reset the colors of the buttons on the board, which can be useful when 
        /// updating the game state or when players move around the board. 
        /// </summary>
        public override void ResetBoardColors()
        {
            for (int row = 0; row < BoardSize; row++)
                for (int col = 0; col < BoardSize; col++)
                    if (!blocked[row, col] && buttons[row, col] != null)
                        buttons[row, col].BackgroundColor = Colors.Transparent;
        }
        /// <summary>
        /// method for restoring the colors of the buttons on the board, it iterates through each cell in 
        /// the grid and calls the RestoreColor method of the button at that cell if the button is not null.
        /// </summary>
        public override void RestoreColors()
        {
            foreach (IndexButton btn in buttons!)
                btn?.RestoreColor();
        }
        /// <summary>
        /// method for updating the color of a button on the board, it takes a Position and a Color as
        /// parameters. The Position parameter specifies the row and column indices of the button to be
        /// updated, and the Color parameter specifies the new background color for the button.
        /// The method retrieves the IndexButton at the specified row and column indices from the buttons
        /// array, and if the button is not null, it sets its BackgroundColor property to the provided 
        /// color. 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="color"></param>
        public override void UpdateButton(Position pos, Color color)
        {
            IndexButton btn = buttons![pos.Row, pos.Column];
            if (btn != null)
                btn.BackgroundColor = color;
        }
        /// <summary>
        /// method for retrieving the button at a specific position on the board, it takes a Position as a
        /// parameter and returns the IndexButton at the specified row and column indices from the buttons
        /// array. The Position parameter specifies the row and column indices of the button to be 
        /// retrieved. 
        /// </summary>
        /// <param name="p"></param>
        public override IndexButton GetButton(Position p)
        {
            return buttons[p.Row, p.Column];
        }
        /// <summary>
        /// method for checking if a specific position on the board is blocked, it takes a Position as a
        /// parameter and returns a boolean value indicating whether the cell at the specified row and 
        /// column indices in the blocked array is true or false. The Position parameter specifies the row
        /// and column indices of the cell to be checked. If the value in the blocked array at the 
        /// specified indices is true, it means that the cell is blocked and players cannot move to that
        /// cell. If the value is false or if the indices are out of bounds, it means that the cell is not
        /// blocked and players can move to that cell. 
        /// </summary>
        /// <param name="p"></param>
        public override bool IsBlocked(Position p)
        {
            return blocked[p.Row, p.Column];
        }
        /// <summary>
        /// method for handling the player's turn, it checks if the rooms have been built and if not, it 
        /// calls the BuildRooms method to create the rooms on the board. The method uses the roomsBuilt 
        /// variable to track whether the rooms have already been built, and it sets this variable to true 
        /// after building the rooms. This method is called at the beginning of each player's turn to ensure
        /// that the rooms are built on the board before any player interactions take place. 
        /// </summary>
        public override void MyTurn()
        {
            if (!roomsBuilt)
            {
                BuildRooms();
                roomsBuilt = true;
            }
        }
        /// <summary>
        /// method for drawing the players on the board, it restores the colors of the buttons and then
        /// updates the buttons for each player with their current position and color.
        /// </summary>
        /// <param name="game"></param>
        public void DrawPlayers(Game game)
        {
            RestoreColors();
            for (int i = 0; i < game.PlayersCount; i++)
                UpdateButton(game.GetPlayerPosition(i), game.GetPlayerColor(i));
        }
    }
}
