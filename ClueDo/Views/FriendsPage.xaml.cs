using ClueDo.ViewModels;

namespace ClueDo.Views;

public partial class FriendsPage : ContentPage
{
	private readonly FriendsPageVM fpVM;
	public FriendsPage(FriendsPageVM fpVM)
	{
		InitializeComponent();
		this.fpVM = fpVM;
		BindingContext = fpVM;
	}
}