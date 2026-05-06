using ClueDo.Models;
using System.Windows.Input;

namespace ClueDo.ViewModels
{

    public partial class CheckPopupVM(bool roomCorrect, bool weaponCorrect, bool suspectCorrect, Action closeAction) : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public string RoomMark { get; } = GetMark(roomCorrect);
        public string WeaponMark { get; } = GetMark(weaponCorrect);
        public string SuspectMark { get; } = GetMark(suspectCorrect);
        public Color RoomColor { get; } = GetColor(roomCorrect);
        public Color WeaponColor { get; } = GetColor(weaponCorrect);
        public Color SuspectColor { get; } = GetColor(suspectCorrect);
        public ICommand CloseCommand { get; } = new Command(closeAction);
        private static string GetMark(bool isCorrect)
        {
            return isCorrect ? Keys.CheckMark : Keys.X;
        }
        private static Color GetColor(bool isCorrect)
        {
            return isCorrect ? Colors.Green : Colors.Red;
        }
    }
}