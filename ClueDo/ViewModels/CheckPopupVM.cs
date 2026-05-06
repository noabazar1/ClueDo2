using ClueDo.Models;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    /// <summary>
    /// class that represents the view model for the check popup, which is used to show the results of a 
    /// suggestion check to the user.
    /// </summary>
    /// <param name="roomCorrect"></param>
    /// <param name="weaponCorrect"></param>
    /// <param name="suspectCorrect"></param>
    /// <param name="closeAction"></param>
    public partial class CheckPopupVM(bool roomCorrect, bool weaponCorrect, bool suspectCorrect, Action closeAction) : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        public string RoomMark { get; } = GetMark(roomCorrect);
        public string WeaponMark { get; } = GetMark(weaponCorrect);
        public string SuspectMark { get; } = GetMark(suspectCorrect);
        public Color RoomColor { get; } = GetColor(roomCorrect);
        public Color WeaponColor { get; } = GetColor(weaponCorrect);
        public Color SuspectColor { get; } = GetColor(suspectCorrect);
        public ICommand CloseCommand { get; } = new Command(closeAction);
        /// <summary>
        /// method that returns the appropriate mark (check or X) based on whether the suggestion was 
        /// correct or not.
        /// </summary>
        /// <param name="isCorrect"></param>
        /// <returns></returns>
        private static string GetMark(bool isCorrect)
        {
            return isCorrect ? Keys.CheckMark : Keys.X;
        }
        /// <summary>
        /// method that returns the appropriate color of the mark (green or red) based on whether the
        /// suggestion was correct or not.
        /// </summary>
        /// <param name="isCorrect"></param>
        /// <returns></returns>
        private static Color GetColor(bool isCorrect)
        {
            return isCorrect ? Colors.Green : Colors.Red;
        }
    }
}