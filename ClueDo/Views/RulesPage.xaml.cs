using ClueDo.ViewModels;

namespace ClueDo.Views;

public partial class RulesPage : ContentPage
{
	public RulesPage()
	{
		InitializeComponent();
		BindingContext = new RulesPageVM();
    }
}