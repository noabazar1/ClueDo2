using System.Windows.Input;
using ClueDo.Models;
using ClueDo.ModelsLogic;

namespace ClueDo.ViewModels
{
    public partial class AuthPageVM : ObservableObject
    {
        private readonly User user = new();
        private readonly ModelsLogic.Connectivity _connectivity = new();
        private bool isAlertShown = false;
        public ICommand AuthCommand { get; }
        public ICommand ToggleIsPasswordCommand { get; }
        public ICommand ShowNoInternetCommand { get; }
        public bool IsBusy => user.IsBusy;
        public bool IsRegistered => user.IsRegistered;
        public bool IsConnected => _connectivity.IsConnected;
        public string UserStateAction => user.IsRegistered ? Strings.Login : Strings.Register;
        public string Name
        {
            get => user.Name;
            set
            {
                if (user.Name != value)
                {
                    user.Name = value;
                    (AuthCommand as Command)?.ChangeCanExecute();
                }
            }
        }
        public string Email
        {
            get => user.Email;
            set
            {
                if (user.Email != value)
                {
                    user.Email = value;
                    (AuthCommand as Command)?.ChangeCanExecute();
                }
            }
        }
        public string Password
        {
            get => user.Password;
            set
            {
                if (user.Password != value)
                {
                    user.Password = value;
                    (AuthCommand as Command)?.ChangeCanExecute();
                }
            }
        }
        public bool IsPassword { get; set; } = true;
        public AuthPageVM()
        {
            AuthCommand = user.IsRegistered ? new Command(Login, CanAuth) : new Command(Register, CanAuth);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            user.OnAuthComplete += OnAuthComplete;
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
            ShowNoInternetCommand = new Command(async () => await ShowNoInternet());
        }
        private async void OnAuthComplete(object? sender, bool success)
        {
            OnPropertyChanged(nameof(IsBusy));

            if (success)
            {
                await Shell.Current.GoToAsync(Keys.MainArea);
            }
        }
        private bool CanAuth()
        {
            return user.IsValid();
        }
        private void Login()
        {
            user.Login();
            OnPropertyChanged(nameof(IsBusy));
        }
        private void Register()
        {
            user.Register();
            OnPropertyChanged(nameof(IsBusy));
        }
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
        private void OnConnectivityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsConnected));
            if (!IsConnected && !isAlertShown)
                ShowNoInternetCommand.Execute(null);
        }
        private async Task ShowNoInternet()
        {
            isAlertShown = true;
            await Shell.Current.DisplayAlert(Strings.NoInternet, Strings.CheckConnection, Strings.Ok);
            isAlertShown = false;
        }
    }
}
