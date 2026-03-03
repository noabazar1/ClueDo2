namespace ClueDo.Models
{
    internal static class Strings
    {
        public const string LogInButton = "log in";
        public const string RegisterButton = "Sign up";
        public const string UsernamePlaceholder = "Username";
        public const string PasswordPlaceholder = "Password";
        public const string CreateUserError = "Error";
        public const string Ok = "Ok";
        public const string UnknownError = "Unknown error";
        public const string Error = "Error";
        public const string Name = "Name";
        public const string Password = "Password";
        public const string Email = "Email";
        public const string Register = "Register";
        public const string Login = "Login";
        public const string JoinGameError = "Error joining game";
        public const string GameDeleted = "Game Deleted";
        public const string AppTitle = "Clue Do";
        public const string AddGame = "Add Game";
        public const string WaitMessage = "Opponent's turn";
        public const string PlayMessage = "Your turn";
        public const string Waiting = "Waiting...";
        public const string Interrogate = "Interrogate";
        public const string MurderWeapon = "Murder Weapon";
        public const string SuspectName = "Suspect Name";
        public const string Send = "Send";
        public const string Wrong = "Wrong";
        public const string NoCorrectGuesses = "No correct guesses";
        public const string CorrectParameters = "The correct parameters:\n";
        public const string Check = "Check";

        public const string Room = "Room";
        public const string Kitchen = "Kitchen";
        public const string Conservatory = "Conservatory";
        public const string Study = "Study";
        public const string Lounge = "Lounge";
        public const string DiningRoom = "Dining Room";
        public const string Ballroom = "Ballroom";
        public const string BilliardRoom = "Billiard Room";
        public const string Library = "Library";
        public const string Hall = "Hall";

        public const string Weapon = "Weapon";
        public const string Knife = "Knife";
        public const string Rope = "Rope";
        public const string Candlestick = "Candlestick";
        public const string LeadPipe = "Lead Pipe";
        public const string Revolver = "Revolver";
        public const string Wrench = "Wrench";

        public const string Suspect = "Suspect";
        public const string MissScarlet = "Miss Scarlet";
        public const string ColonelMustard = "Colonel Mustard";
        public const string ProfessorPlum = "Professor Plum";
        public const string MrsWhite = "Mrs. White";
        public const string ReverendGreen = "Reverend Green";
        public const string MrsPeacock = "Mrs. Peacock";

        public const string TheRoom = "The room\n";
        public const string TheMurderWeapon = "The murder weapon\n";
        public const string TheSuspect = "The suspect\n";

        public const string Rules1 = "When the game begins, wait until it is your turn." +
            " You can act only during your own turn. When your turn starts, press the dice button to roll." +
            " The total result of the two dice determines how many steps you may move during that turn." +
            " This number becomes your available movement points.";
        public const string Rules2 = "Move across the board one tile at a time." +
            " You may move only to an adjacent square horizontally or vertically." +
            " Diagonal movement is not allowed. Each step uses one movement point." +
            " You cannot move if you have no remaining movement points." +
            " When your movement points reach zero, your turn ends automatically and the next player " +
            "begins their turn.";
        public const string Rules3 = "If you reach a door tile while you still have movement points, you may" +
            " enter the room. After entering a room, you must make an accusation by choosing a suspect and" +
            " a weapon. The room is determined by the door you entered.";
        public const string Rules4 = "Once you submit your accusation, the game checks whether the" +
            " suspect, the weapon, and the room match the hidden solution. If all three are correct," +
            " the game ends immediately and you win. If one or more elements are incorrect, the results" +
            " are shown and your turn ends automatically.";
        public const string Rules5 = "The objective of the game is to be the first player to correctly" +
            " identify the hidden suspect, weapon, and room combination.";
        public const string Rules6 = "You can manage your friends in a separate Friends page within the" +
            " application. From this page, you may add a friend by selecting a contact from your phone." +
            " The app will request permission to access your contacts. Once permission is granted," +
            " you can choose a contact and add them to your friends list. A contact that already exists" +
            " in your list cannot be added again. You may also remove friends from your list at any time" +
            " from this page.";
        public const string Rules7 = "During an active game, an emergency challenge may begin if one of" +
            " the friends from your friends list calls you. When this happens, a popup immediately appears" +
            " with a ten-second countdown. Your objective is to press the red button twenty times before" +
            " the timer reaches zero. The screen displays both the remaining time and the number of presses" +
            " you have completed. If you press the button twenty times within ten seconds, the challenge" +
            " closes successfully. If the timer reaches zero before you complete twenty presses, the" +
            " challenge closes automatically. The challenge can occur only while a game is in progress.";
    }
}
