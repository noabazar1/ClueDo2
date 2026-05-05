using Plugin.CloudFirestore.Attributes;
using ClueDo.ModelsLogic;

namespace ClueDo.Models
{
    /// <summary>
    /// class that holds the data of the players, and the methods to add players, get player names, check 
    /// if it's the opponent's turn, set the next player, check if it's the player's turn, and play a move.
    /// This class is used in the Game Page to manage the players in the game, and to update the players' 
    /// information and status during the game.
    /// </summary>
    public abstract class PlayersModel
    {
        [Ignored]
        public int MyIndex { get; set; } = 0;
        [Ignored]
        public int Count => PlayersList.Count;
        public List<Player> PlayersList { get; set; } = [];
        public int NextPlay { get; set; }
        public int TotalPlayers = 4;
        /// <summary>
        /// abstract method to add a player to the players list. This method takes a Player object as a 
        /// parameter, and it is used to add a player to the game. The implementation of this method will
        /// depend on the specific requirements of the game, such as how the players are added, and how 
        /// the player's information is stored and updated in the game. This method is called when a new
        /// player joins the game, and it is used to update the players list and the player's information 
        /// in the game.
        /// </summary>
        /// <param name="p"></param>
        public abstract void Add(Player p);
        /// <summary>
        /// abstract method to get the name of a player based on their index in the players list. 
        /// This method takes an integer index as a parameter, and it returns the name of the player at 
        /// that index in the players list. The implementation of this method will depend on how the 
        /// player's information is stored in the game, and how the player's name is retrieved from the 
        /// player's information.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract string GetPlayerName(int index);
        /// <summary>
        /// abstract method to check if it's the opponent's turn based on their index in the players list.
        /// This method takes an integer index as a parameter, and it returns a boolean value indicating 
        /// whether it's the opponent's turn or not. 
        /// </summary>
        /// <param name="opponentIndex"></param>
        /// <returns></returns>
        public abstract bool IsOpponentTurn(int opponentIndex);
        /// <summary>
        /// abstract method to set the next player in the game. This method is called when a player 
        /// finishes their turn, and it is used to update the next player in the game. The implementation 
        /// of this method will depend on how the players are managed in the game, and how the next player 
        /// is determined based on the current player's index and the total number of players in the game. 
        /// </summary>
        public abstract void SetNextPlayer();
    }
}
