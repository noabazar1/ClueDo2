namespace ClueDo.Models
{
    public class GameSize
    {
        public int Size { get; set; }
        public string DisplayName => $"{Size} x {Size}";
        public GameSize(int size)
        {
            Size = size;
        }
        public GameSize()
        {
            Size = 3;
        }
    }
}
