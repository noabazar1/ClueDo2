using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    /// <summary>
    /// class that manages the players in the game, allowing users to see their name and color, and to keep 
    /// track of the next player to play. The Players class inherits from the PlayersModel class, which
    /// defines the properties and methods for managing the players in the game. The Players class 
    /// implements the methods for adding a player to the game, getting a player's name by index, setting 
    /// the next player to play, and checking if it is an opponent's turn. 
    /// </summary>
    public class Players : PlayersModel
    {
        /// <summary>
        /// constructor for the Players class, which initializes the list of players and sets the next
        /// player index to 0. 
        /// </summary>
        public Players() { }
        /// <summary>
        /// method to add a player to the game, which is called when a new player joins the game. The method
        /// takes a Player object as a parameter and adds it to the PlayersList. The PlayersList is a list
        /// of Player objects that represents all the players in the game, including the current player and
        /// the opponents.
        /// </summary>
        /// <param name="p"></param>
        public override void Add(Player p)
        {
            PlayersList.Add(p);
        }       
        /// <summary>
        /// method to check if it is an opponent's turn, which is used to determine if the current player 
        /// should be able to make a move or not. The method takes an opponent index as a parameter and 
        /// returns a boolean value indicating whether it is the opponent's turn or not. The method compares
        /// the opponent index with the NextPlay property, which keeps track of the index of the next player
        /// to play. If the opponent index matches the NextPlay index, it means that it is the opponent's 
        /// turn, and the method returns true; otherwise, it returns false. This method is important for 
        /// managing the game flow and ensuring that players can only make moves when it is their turn to 
        /// play.
        /// </summary>
        /// <param name="opponentIndex"></param>
        /// <returns></returns>
        public override bool IsOpponentTurn(int opponentIndex)
        {
            return opponentIndex == NextPlay;
        }
    }
}
