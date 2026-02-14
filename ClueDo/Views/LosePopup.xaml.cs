using CommunityToolkit.Maui.Views;

namespace ClueDo.Views;

public partial class LosePopup : Popup
{
	public LosePopup(string winnerName)
	{
		InitializeComponent();
	}
    private async void OnBackClicked(object sender, EventArgs e)
    {
        Close();
        await Shell.Current.GoToAsync("//MainPage");
    }
}