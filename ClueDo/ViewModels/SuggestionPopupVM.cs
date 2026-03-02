using ClueDo.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            Weapons = new List<string>
            {
                "Knife", "Rope", "Candlestick", "Lead Pipe", "Revolver", "Wrench"
            };

            Suspects = new List<string>
            {
                "Miss Scarlet", "Colonel Mustard", "Professor Plum",
                "Mrs. White", "Reverend Green", "Mrs. Peacock"
            };

            ConfirmCommand = new Command(OnConfirm);
        }

        private void OnConfirm()
        {
            if (SelectedWeapon == null || SelectedSuspect == null)
                return;

            Accusation accusation = new Accusation
            {
                Room = Room,
                Weapon = SelectedWeapon,
                Suspect = SelectedSuspect
            };

            popup.Close(accusation);
        }
    }
}
