using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    /// <summary>
    /// class that manages the user's information, allowing users to register and log in to the app. The 
    /// User class inherits from the UserModel class, which defines the properties and methods for managing
    /// the user's information.
    /// </summary>
    public class User : UserModel
    {
        /// <summary>
        /// constructor for the User class, which initializes the user's information by retrieving it from
        /// the device's preferences. The constructor uses the Preferences class to get the user's name, 
        /// email, password, and Firebase user ID from the device's storage. If the information is not 
        /// found in the preferences, it initializes the properties with empty strings.
        /// </summary>
        public User()
        {
            Name = Preferences.Get(Keys.NameKey, string.Empty);
            Email = Preferences.Get(Keys.EmailKey, string.Empty);
            Password = Preferences.Get(Keys.PasswordKey, string.Empty);
            FirebaseUserId = Preferences.Get(Keys.FirebaseUserId, string.Empty);
        }
        /// <summary>
        /// method to register a new user, which is called when a user tries to create a new account. The 
        /// method sets the IsBusy property to true while the registration process is ongoing, and it calls
        /// the CreateUser method of the FirebaseDatabase class to create a new user with the provided 
        /// email, password, and name. The OnComplete method is called when the registration task is 
        /// completed to handle the result of the operation. If the registration is successful, the user's
        /// information is saved to the device's preferences, and the OnAuthComplete event is raised with a
        /// value of true to indicate that the authentication process was successful. If there is an error 
        /// during registration, an alert is shown with the error message, and the OnAuthComplete event is 
        /// raised with a value of false to indicate that the authentication process failed.
        /// </summary>
        public override void Register()
        {
            IsBusy = true;
            CurrentAction = Actions.Register;
            fbd.CreateUserWithEmailAndPasswordAsync(Email, Password, Name, OnComplete);
        }
        /// <summary>
        /// method to log in an existing user, which is called when a user tries to access their account.
        /// The method sets the IsBusy property to true while the login process is ongoing, and it calls the
        /// SignIn method of the FirebaseDatabase class to authenticate the user with the provided email
        /// and password. The OnComplete method is called when the login task is completed to handle the
        /// result of the operation. If the login is successful, the user's information is saved to the 
        /// device's preferences, and the OnAuthComplete event is raised with a value of true to indicate 
        /// that the authentication process was successful. If there is an error during login, an alert is 
        /// shown with the error message, and the OnAuthComplete event is raised with a value of false to 
        /// indicate that the authentication process failed.
        /// </summary>
        public override void Login()
        {
            IsBusy = true;
            fbd.SignInWithEmailAndPasswordAsync(Email, Password, OnComplete);
        }
        /// <summary>
        /// method to check if the user's input is valid, which is called before attempting to register or 
        /// log in the user. The method checks if the Name, Email, and Password properties are not null or 
        /// whitespace and returns true if all the properties are valid; otherwise, it returns false. This 
        /// method is important for ensuring that the user provides the necessary information for 
        /// authentication and for preventing errors during the registration and login processes.
        /// </summary>
        /// <returns></returns>
        public override bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name)
                && !string.IsNullOrWhiteSpace(Password)
                && !string.IsNullOrWhiteSpace(Email);
        }
        /// <summary>
        /// method to handle the completion of tasks related to user registration and login. This method is 
        /// called when a registration or login task is completed, and it checks the result of the task to
        /// determine whether the operation was successful or if there was an error. If the task is 
        /// completed successfully, it saves the user's information to the device's preferences and raises
        /// the OnAuthComplete event with a value of true to indicate that the authentication process was 
        /// successful.
        /// </summary>
        /// <param name="task"></param>
        private void OnComplete(Task task)
        {
            IsBusy = false;
            if (task.IsCompletedSuccessfully)
            {
                Preferences.Set(Keys.FirebaseUserId, FirebaseUserId);
                if (CurrentAction == Actions.Register)
                    SaveToPreferences();
                OnAuthComplete?.Invoke(this, true);
            }
            else if (task.Exception != null)
            {
                string errMessage = task.Exception.Message;
                ShowAlert(errMessage);
                OnAuthComplete?.Invoke(this, false);
            }
            else
                ShowAlert(Strings.UnknownError);
        }
        /// <summary>
        /// method to show an alert with an error message, which is called when there is an error during the
        /// registration or login processes. The method takes an error message as a parameter, retrieves a
        /// user-friendly error message using the FirebaseDatabase class, and displays it as a toast 
        /// notification to the user. The method uses the MainThread class to ensure that the toast 
        /// notification is displayed on the main thread, which is necessary for updating the UI in a
        /// mobile application. 
        /// </summary>
        /// <param name="errMessage"></param>
        private void ShowAlert(string errMessage)
        {
            errMessage = fbd.GetErrorMessage(errMessage);
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(errMessage, ToastDuration.Long).Show();
            });
        }
        /// <summary>
        /// method to save the user's information to the device's preferences, which is called after a 
        /// successful registration or login. The method uses the Preferences class to store the user's 
        /// name, email, and password in the device's storage. 
        /// </summary>
        private void SaveToPreferences()
        {
            Preferences.Set(Keys.NameKey, Name);
            Preferences.Set(Keys.EmailKey, Email);
            Preferences.Set(Keys.PasswordKey, Password);
        }
    }
}