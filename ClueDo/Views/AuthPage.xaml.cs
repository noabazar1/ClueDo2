using ClueDo.ViewModels;

namespace ClueDo.Views;

public partial class AuthPage : ContentPage
{
	public AuthPage()
	{
		InitializeComponent();
		BindingContext = new AuthPageVM();
	}
}