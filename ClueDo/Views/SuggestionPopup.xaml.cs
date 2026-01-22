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
        Close(true);
    }
}
