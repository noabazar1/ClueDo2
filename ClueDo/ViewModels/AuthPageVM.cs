using System.Windows.Input;
using ClueDo.Models;
using ClueDo.ModelsLogic;

namespace ClueDo.ViewModels
{
    /// <summary>
    /// class that serves as the ViewModel for the authentication page of the application. It manages the 
    /// state and logic related to user authentication, including handling user input for name, email, and
    /// password, managing the authentication process (login and registration), and responding to changes
    /// in network connectivity. The ViewModel also provides commands for the view to bind to, allowing for
    /// user interactions such as toggling password visibility and showing alerts when there is no internet
    /// connection.
    /// </summary>
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
        /// <summary>
        /// constructor for the AuthPageVM class, which initializes the commands for authentication and
        /// toggling password visibility, subscribes to the OnAuthComplete event of the User class to
        /// handle authentication results, and subscribes to the ConnectivityChanged event of the
        /// Connectivity class to respond to changes in network connectivity. The constructor also 
        /// initializes the ShowNoInternetCommand to display an alert when there is no internet connection.
        /// </summary>
        public AuthPageVM()
        {
            AuthCommand = user.IsRegistered ? new Command(Login, CanAuth) : new Command(Register, CanAuth);
            ToggleIsPasswordCommand = new Command(ToggleIsPassword);
            user.OnAuthComplete += OnAuthComplete;
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
            ShowNoInternetCommand = new Command(async () => await ShowNoInternet());
        }
        /// <summary>
        /// method that is called when the authentication process is complete, which updates the IsBusy 
        /// property to indicate that the authentication process has finished and checks if the 
        /// authentication was successful. If the authentication was successful, it navigates to the main 
        /// area of the application using Shell navigation. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="success"></param>
        private async void OnAuthComplete(object? sender, bool success)
        {
            OnPropertyChanged(nameof(IsBusy));

            if (success)
            {
                await Shell.Current.GoToAsync(Keys.MainArea);
            }
        }
        /// <summary>
        /// method that checks if the user can proceed with authentication (either login or registration)
        /// by validating the user input. It calls the IsValid method of the User class, which  
        /// checks if the name, email, and password fields are properly filled out and meet any necessary
        /// criteria. 
        /// </summary>
        /// <returns></returns>
        private bool CanAuth()
        {
            return user.IsValid();
        }
        /// <summary>
        /// method that handles the login process by calling the Login method of the User class. It also 
        /// updates the IsBusy property to indicate that the authentication process is in progress. The 
        /// Login method of the User class is responsible for performing the actual authentication logic, 
        /// such as validating the user's credentials and communicating with a backend service if necessary.
        /// After the Login method is called, the OnAuthComplete event will be triggered once the 
        /// authentication process is complete, allowing the ViewModel to respond accordingly.
        /// </summary>
        private void Login()
        {
            user.Login();
            OnPropertyChanged(nameof(IsBusy));
        }
        /// <summary>
        /// method that handles the registration process by calling the Register method of the User class. 
        /// Similar to the Login method, it updates the IsBusy property to indicate that the authentication
        /// process is in progress and relies on the User class to perform the actual registration logic.
        /// The Register method of the User class will handle tasks such as validating the user's input, 
        /// creating a new user account, and communicating with a backend service if necessary. Once the 
        /// registration process is complete, the OnAuthComplete event will be triggered to allow the 
        /// ViewModel to respond to the result of the registration attempt.
        /// </summary>
        private void Register()
        {
            user.Register();
            OnPropertyChanged(nameof(IsBusy));
        }
        /// <summary>
        /// method that toggles the visibility of the password input field. It changes the value of the 
        /// IsPassword property, which is bound to the view to determine whether the password should be
        /// displayed as plain text or masked. When the ToggleIsPasswordCommand is executed, this method is
        /// called to update the IsPassword property and notify the view of the change, allowing the UI to 
        /// update accordingly. 
        /// </summary>
        private void ToggleIsPassword()
        {
            IsPassword = !IsPassword;
            OnPropertyChanged(nameof(IsPassword));
        }
        /// <summary>
        /// method that is called when there is a change in network connectivity. It updates the IsConnected
        /// property to reflect the current connectivity status and checks if the device is not connected to
        /// the internet. If there is no internet connection and an alert has not already been shown, it 
        /// executes the ShowNoInternetCommand to display an alert to the user, informing them about the 
        /// lack of internet connectivity and prompting them to check their connection. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectivityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsConnected));
            if (!IsConnected && !isAlertShown)
                ShowNoInternetCommand.Execute(null);
        }
        /// <summary>
        /// method that displays an alert to the user when there is no internet connection. It sets the 
        /// isAlertShown flag to true to prevent multiple alerts from being shown simultaneously, and it 
        /// uses the Shell.Current.DisplayAlert method to show an alert with a title, message, and an "OK"
        /// button. The alert informs the user about the lack of internet connectivity and prompts them to
        /// check their connection. Once the user dismisses the alert, the isAlertShown flag is set back to
        /// false, allowing future alerts to be shown if the connectivity changes again. This method is 
        /// called when the OnConnectivityChanged event detects that there is no internet connection and an
        /// alert has not already been shown.
        /// </summary>
        /// <returns></returns>
        private async Task ShowNoInternet()
        {
            isAlertShown = true;
            await Shell.Current.DisplayAlert(Strings.NoInternet, Strings.CheckConnection, Strings.Ok);
            isAlertShown = false;
        }
    }
}
