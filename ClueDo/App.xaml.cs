using ClueDo.Views;

namespace ClueDo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AuthPage();
        }
    }
}
