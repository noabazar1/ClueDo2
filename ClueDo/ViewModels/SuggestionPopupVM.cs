using ClueDo.Models;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    public class SuggestionPopupVM
    {
        public string Room { get; }
        public List<string> Weapons { get; }
        public List<string> Suspects { get; }
        public string? SelectedWeapon { get; set; }
        public string? SelectedSuspect { get; set; }
        public ICommand ConfirmCommand { get; }

        private readonly Popup popup;
        public SuggestionPopupVM(string room, Popup popup)
        {
            Room = room;
            this.popup = popup;
            Weapons =
            [
                Strings.Knife, Strings.Rope, Strings.Candlestick, Strings.LeadPipe, Strings.Revolver,
                Strings.Wrench
            ];
            Suspects =
            [
                Strings.MissScarlet, Strings.ColonelMustard, Strings.ProfessorPlum,
                Strings.MrsWhite, Strings.ReverendGreen, Strings.MrsPeacock
            ];
            ConfirmCommand = new Command(OnConfirm);
        }
        private void OnConfirm()
        {
            if (SelectedWeapon != null && SelectedSuspect != null)
            {
                Accusation accusation = new()
                {
                    Room = Room,
                    Weapon = SelectedWeapon,
                    Suspect = SelectedSuspect
                };
                popup.Close(accusation);
            }
        }
    }
}
