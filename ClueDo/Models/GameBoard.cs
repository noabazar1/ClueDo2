using ClueDo.Models;
using Microsoft.Maui.Controls;

namespace ClueDo.ModelsLogic
{
    public class GameBoard
    {
        private const int BoardSize = 15;
        private bool roomsBuilt = false;
        private bool isBuilt = false;

        private readonly IndexButton[,] buttons;
        private readonly bool[,] blocked;
        public GameBoard()
        {
            buttons = new IndexButton[BoardSize, BoardSize];
            blocked = new bool[BoardSize, BoardSize];
        }
        public void Build(Grid board, EventHandler clickHandler)
        {
            if (isBuilt)
                return;

            isBuilt = true;

            CreateGrid(board);
            CreateButtons(board, clickHandler);
            BuildRooms();
        }

        public static void CreateGrid(Grid board)
        {
            board.Children.Clear();
            board.RowDefinitions.Clear();
            board.ColumnDefinitions.Clear();

            for (int i = 0; i < BoardSize; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
        }
        public void CreateButtons(Grid board, EventHandler clickHandler)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    IndexButton button = new IndexButton(row, col);
                    button.Clicked += clickHandler;

                    buttons[row, col] = button;
                    board.Add(button, col, row);
                }
            }
        }
        
        public void BlockArea(int rowStart, int rowEnd, int colStart, int colEnd)
        {
            for (int row = rowStart; row <= rowEnd; row++)
            {
                for (int col = colStart; col <= colEnd; col++)
                {
                    blocked[row, col] = true;

                    buttons[row, col].IsEnabled = false;
                    buttons[row, col].BackgroundColor = Colors.LightCoral;
                }
            }
        }
        public void BuildRooms()
        {
            BlockArea(11, 14, 11, 14); // Kitchen
            MakeDoor(11, 11);
            BlockArea(0, 3, 11, 14);  // Conservatory
            MakeDoor(3, 11);
            BlockArea(0, 4, 0, 2);    // Study
            MakeDoor(4, 2);
            BlockArea(11, 14, 0, 3);  // Lounge
            MakeDoor(11, 3);
            BlockArea(10, 14, 6, 9);  // Dining Room
            MakeDoor(10, 7);
            BlockArea(5, 9, 11, 13);  // Ballroom
            BlockArea(6, 8, 14, 14);  // Ballroom side
            BlockArea(5, 8, 10, 10);  // Ballroom side
            MakeDoor(7, 10);
            BlockArea(0, 3, 8, 9);    // Billiard Room
            MakeDoor(3, 8);
            BlockArea(0, 4, 5, 6);    // Library
            MakeDoor(4, 5);
            BlockArea(6, 9, 0, 4);    // Hall
            MakeDoor(7, 4);
        }
        public void MakeDoor(int row, int col)
        {
            IndexButton btn = buttons[row, col];
            btn.IsEnabled = true;
            btn.BackgroundColor = Colors.LightPink;
        }
        public void ResetBoardColors()
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (!blocked[row, col])
                    {
                        buttons[row, col].BackgroundColor = Color.FromArgb("#F7D275");
                    }
                }
            }
        }
        public void RestoreColors()
        {
            foreach (IndexButton btn in buttons!)
                if (btn != null)
                    btn.RestoreColor();
        }
        public void UpdateButton(Position pos, Color color)
        {
            IndexButton btn = buttons![pos.Row, pos.Column];
            if (btn != null)
                btn.BackgroundColor = color;
        }
        public IndexButton GetButton(Position p)
        {
            return buttons[p.Row, p.Column];
        }
        public bool IsBlocked(Position p)
        {
            return blocked[p.Row, p.Column];
        }
        public void MyTurn()
        {
            if (!roomsBuilt)
            {
                BuildRooms();
                roomsBuilt = true;
            }
        }
        public void OpponentTurn()
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
    }
}
