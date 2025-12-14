namespace ClueDo.Models
{
    public class Player
    {
        public string Name { get; }
        public Position Position { get; private set; }

        public Player(string name, Position startPosition)
        {
            Name = name;
            Position = startPosition;
        }
        public Player()
        {
            Name = string.Empty;
            Position = new Position(0, 0);
        }
        public void MoveTo(Position newPosition)
        {
            Position = newPosition;
        }
    }
}
