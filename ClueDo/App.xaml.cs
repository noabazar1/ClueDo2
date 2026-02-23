using ClueDo.Views;

namespace ClueDo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            string userId = Preferences.Get("UserId", string.Empty);
            if (string.IsNullOrEmpty(userId) )
            {
                userId = Guid.NewGuid().ToString();
                Preferences.Set("UserId", userId);
            }
            MainPage = new AuthPage();
        }
    }
}
