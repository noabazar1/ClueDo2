using CommunityToolkit.Maui.Views;
using System.Threading.Tasks;

namespace ClueDo.Views;

public partial class VictoryPopup : Popup
{
	public VictoryPopup()
	{
		InitializeComponent();
	}
	private async void OnBackClicked(object sender, EventArgs e)
	{
		Close();
		await Shell.Current.GoToAsync("//MainPage");
	}
}