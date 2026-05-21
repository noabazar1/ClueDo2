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
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is FriendsPageVM vm)
        {
            await vm.LoadFriends();
        }
    }
}