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

        FriendsPageVM? vm = BindingContext as FriendsPageVM;

        if (vm != null)
        {
            await vm.LoadFriends();
        }
    }

}