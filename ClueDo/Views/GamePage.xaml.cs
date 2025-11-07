using ClueDo.ModelsLogic;
using ClueDo.ViewModels;
namespace ClueDo.Views;

public partial class GamePage : ContentPage
{
	public GamePage(Game game)
	{
		InitializeComponent();
		BindingContext = new GamePageVM(game);
	}
}