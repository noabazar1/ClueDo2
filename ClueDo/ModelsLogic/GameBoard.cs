using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    public class GameBoard : GameBoardModel
    {
        public IndexButton[,] buttons;
        public bool[,] blocked;
        public GameBoard()
        {
            buttons = new IndexButton[BoardSize, BoardSize];
            blocked = new bool[BoardSize, BoardSize];
        }
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
        public override void MakeDoor(int row, int col, string roomName)
        {
            IndexButton btn = buttons[row, col];
            btn.IsEnabled = true;
            btn.IsDoor = true;
            btn.RoomName = roomName;
        }
        public override void ResetBoardColors()
        {
            for (int row = 0; row < BoardSize; row++)
                for (int col = 0; col < BoardSize; col++)
                    if (!blocked[row, col] && buttons[row, col] != null)
                        buttons[row, col].BackgroundColor = Colors.Transparent;
        }
        public override void RestoreColors()
        {
            foreach (IndexButton btn in buttons!)
                btn?.RestoreColor();
        }
        public override void UpdateButton(Position pos, Color color)
        {
            IndexButton btn = buttons![pos.Row, pos.Column];
            if (btn != null)
                btn.BackgroundColor = color;
        }
        public override IndexButton GetButton(Position p)
        {
            return buttons[p.Row, p.Column];
        }
        public override bool IsBlocked(Position p)
        {
            return blocked[p.Row, p.Column];
        }
        public override void MyTurn()
        {
            if (!roomsBuilt)
            {
                BuildRooms();
                roomsBuilt = true;
            }
        }
        public override void OpponentTurn()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (!blocked[row, col])
                    {
                        blocked[row, col] = true;
                        BlockArea(row, row, col, col);
                    }
                }
            }
        }
        public void DrawPlayers(Game game)
        {
            RestoreColors();
            for (int i = 0; i < game.PlayersCount; i++)
                UpdateButton(game.GetPlayerPosition(i), game.GetPlayerColor(i));
        }
    }
}
