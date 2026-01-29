using ClueDo.Models;
using ClueDo.ModelsLogic;
using CommunityToolkit.Maui.Views;

namespace ClueDo.Views;

public partial class SuggestionPopup : Popup
{
    private readonly string roomName;
    public SuggestionPopup(string roomName)
    {
        InitializeComponent();
        this.roomName = roomName;
        RoomPicker.ItemsSource = new List<string> { roomName };
        RoomPicker.SelectedIndex = 0;
        RoomPicker.IsEnabled = false; 

        WeaponPicker.ItemsSource = new List<string>
        {
            "Knife", "Rope", "Candlestick", "Lead Pipe", "Revolver", "Wrench"
        };

        SuspectPicker.ItemsSource = new List<string>
        {
            "Miss Scarlet", "Colonel Mustard", "Professor Plum", "Mrs. White", "Reverend Green", "Mrs. Peacock"
        };
    }

    private void OnConfirm(object sender, EventArgs e)
    {
        string? room = RoomPicker.SelectedItem as string;
        string? weapon = WeaponPicker.SelectedItem as string;
        string? suspect = SuspectPicker.SelectedItem as string;

        if (room == null || weapon == null || suspect == null)
        {
            return;
        }

        Accusation accusation = new Accusation
        {
            Room = room,
            Weapon = weapon,
            Suspect = suspect
        };

        Close(accusation);
    }

}
