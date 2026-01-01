using ClueDo.Models;
using Microsoft.Maui.Controls;

namespace ClueDo.ModelsLogic
{
    public class GameBoard
    {
        private const int BoardSize = 15;

        private readonly IndexButton[,] buttons;
        private readonly bool[,] blocked;
        public GameBoard()
        {
            buttons = new IndexButton[BoardSize, BoardSize];
            blocked = new bool[BoardSize, BoardSize];
        }
        public void Build(Grid board, EventHandler clickHandler)
        {
            CreateGrid(board);
            CreateButtons(board, clickHandler);
            BuildRooms();
        }
        private void CreateGrid(Grid board)
        {
            board.RowDefinitions.Clear();
            board.ColumnDefinitions.Clear();

            for (int i = 0; i < BoardSize; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
        }
        private void CreateButtons(Grid board, EventHandler clickHandler)
        {
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    IndexButton button = new IndexButton(row, col);

                    button.Text = $"{row},{col}";

                    button.Clicked += clickHandler;

                    buttons[row, col] = button;
                    board.Add(button, col, row);
                }
            }
        }
        private void BlockArea(int rowStart, int rowEnd, int colStart, int colEnd)
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
        private void BuildRooms()
        {
            BlockArea(11, 14, 11, 14); // Kitchen
            BlockArea(0, 3, 11, 14);  // Conservatory
            BlockArea(0, 4, 0, 2);    // Study
            BlockArea(11, 14, 0, 3);  // Lounge
            BlockArea(10, 14, 6, 9);  // Dining Room
            BlockArea(5, 9, 11, 13);  // Ballroom
            BlockArea(6, 8, 14, 14);  // Ballroom side
            BlockArea(5, 8, 10, 10);  // Ballroom side
            BlockArea(0, 3, 8, 9);    // Billiard Room
            BlockArea(0, 4, 5, 6);    // Library
            BlockArea(6, 9, 0, 4);    // Hall
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
            BuildRooms();
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
