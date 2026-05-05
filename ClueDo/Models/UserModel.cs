using ClueDo.ModelsLogic;

namespace ClueDo.Models
{
    /// <summary>
    /// class that holds the data of the user, and the methods to register and login the user. This class
    /// is used in the LoginPage to manage the user's authentication, and to update the user's information 
    /// and status during the authentication process. The UserModel class has properties for the user's 
    /// name, email, password, and Firebase user ID, as well as methods for registering and logging in the
    /// user, and checking if the user's input is valid. 
    /// </summary>
    public abstract class UserModel
    {
        protected FbData fbd = new();
        protected enum Actions { Register, Login }
        protected Actions CurrentAction = Actions.Login;
        public EventHandler<bool>? OnAuthComplete;
        public bool IsRegistered => !string.IsNullOrWhiteSpace(Name);
        public bool IsBusy { get; protected set; } = false;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FirebaseUserId { get; set; }
        /// <summary>
        /// abstract method to register the user, which will be implemented in the User class. This method
        /// is used to create a new user account in the authentication system, and to update the user's 
        /// information and status during the registration process. 
        /// </summary>
        public abstract void Register();
        /// <summary>
        /// abstract method to login the user, which will be implemented in the User class. This method is
        /// used to authenticate the user with the provided email and password, and to update the user's 
        /// information and status during the login process.  
        /// </summary>
        public abstract void Login();
        /// <summary>
        /// abstract method to check if the user's input is valid, which will be implemented in the User 
        /// class.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsValid();
    }
}