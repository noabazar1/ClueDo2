using ClueDo.Models;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    /// <summary>
    /// class that manages the data and logic for the suggestion popup, allowing users to make a suggestion
    /// about the room, weapon, and suspect involved in the crime. The SuggestionPopupVM class contains 
    /// properties for the room, weapons, suspects, and selected weapon and suspect. It also has a command
    /// for confirming the suggestion, which is executed when the user clicks the confirm button in the 
    /// popup.
    /// </summary>
    public class SuggestionPopupVM
    {
        public string Room { get; }
        public List<string> Weapons { get; }
        public List<string> Suspects { get; }
        public string? SelectedWeapon { get; set; }
        public string? SelectedSuspect { get; set; }
        public ICommand ConfirmCommand { get; }

        private readonly Popup popup;
        /// <summary>
        /// constructor for the SuggestionPopupVM class, which initializes the properties and commands for
        /// the suggestion popup. The constructor takes the room name and the popup instance as parameters,
        /// and it initializes the list of weapons and suspects with the predefined values. The 
        /// ConfirmCommand is initialized to execute the OnConfirm method when the user clicks the confirm 
        /// button in the popup. 
        /// </summary>
        /// <param name="room"></param>
        /// <param name="popup"></param>
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
        /// <summary>
        /// method that is called when the user clicks the confirm button in the suggestion popup. This 
        /// method checks if the user has selected a weapon and a suspect, and if so, it creates an 
        /// Accusation object with the selected room, weapon, and suspect. The Accusation object is then 
        /// passed to the Close method of the popup to close the popup and return the accusation as a 
        /// result.
        /// </summary>
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
