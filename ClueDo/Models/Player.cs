namespace ClueDo.Models
{
    public class Player
    {
        public string Name { get; }
        public Point Point { get; private set; }
        public Color Color { get; }


        public Player(string name, Point startPoint, Color color)
        {
            Name = name;
            Point = startPoint;
            Color = color;
        }
        public Player()
        {
            Name = string.Empty;
            Point = new Point(0, 0);
            Color = Colors.Transparent;
        }
        public void MoveTo(Point newPoint)
        {
            Point = newPoint;
        }
    }
}
