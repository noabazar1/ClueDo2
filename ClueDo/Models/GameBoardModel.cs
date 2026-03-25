namespace ClueDo.Models
{
    public abstract class GameBoardModel
    {
        public const int BoardSize = 15;
        public bool roomsBuilt = false;
        public bool isBuilt = false;
        public abstract void Build(Grid board, EventHandler clickHandler);
        public abstract void CreateGrid(Grid board);
        public abstract void CreateButtons(Grid board, EventHandler clickHandler);
        public abstract void BlockArea(int rowStart, int rowEnd, int colStart, int colEnd);
        public abstract void BuildRooms();
        public abstract void MakeDoor(int row, int col, string roomName);
        public abstract void ResetBoardColors();
        public abstract void RestoreColors();
        public abstract void UpdateButton(Position pos, Color color);
        public abstract IndexButton GetButton(Position p);
        public abstract bool IsBlocked(Position p);
        public abstract void MyTurn();
        public abstract void OpponentTurn();
    }
}
