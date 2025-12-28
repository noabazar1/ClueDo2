namespace ClueDo.Models
{
    public class Player
    {
        public string Name { get; }
        public int Index { get; }
        public Position Position { get; private set; }
        public Color Color { get; }

        public Player(string name, int index, Position startPosition, Color color)
        {
            Name = name;
            Index = index;
            Position = startPosition;
            Color = color;
        }

        public Player()
        {
            Name = string.Empty;
            Index = 0;
            Position = new Position(0, 0);
            Color = Colors.Transparent;
        }

        public void MoveTo(Position newPosition)
        {
            Position = newPosition;
        }
    }
}
