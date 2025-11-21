using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClueDo.Models
{
    public class GamePiece : INotifyPropertyChanged
    {
        private int x;
        private int y;
        public int X
        {
            get => x;
            set { x = value; OnPropertyChanged(); }
        }
        public int Y
        {
            get => y;
            set { y = value; OnPropertyChanged(); }
        }
        public string Color { get; set; } = "Red";
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
