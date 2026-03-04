using ClueDo.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    public partial class CheckPopupVM : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public string RoomMark { get; }
        public string WeaponMark { get; }
        public string SuspectMark { get; }
        public Color RoomColor { get; }
        public Color WeaponColor { get; }
        public Color SuspectColor { get; }
        public ICommand CloseCommand { get; }
        public CheckPopupVM(bool roomCorrect, bool weaponCorrect, bool suspectCorrect, Action closeAction)
        {
            RoomMark = GetMark(roomCorrect);
            WeaponMark = GetMark(weaponCorrect);
            SuspectMark = GetMark(suspectCorrect);
            RoomColor = GetColor(roomCorrect);
            WeaponColor = GetColor(weaponCorrect);
            SuspectColor = GetColor(suspectCorrect);
            CloseCommand = new Command(closeAction);
        }
        private string GetMark(bool isCorrect)
        {
            return isCorrect ? Keys.CheckMark : Keys.X;
        }
        private Color GetColor(bool isCorrect)
        {
            return isCorrect ? Colors.Green : Colors.Red;
        }
    }
}