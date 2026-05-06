using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    /// <summary>
    /// class that manages the player's information in the game, allowing users to see their name and color.
    /// The Player class inherits from the PlayerModel class, which defines the properties and methods for 
    /// managing the player's information. The Player class implements the constructors for initializing the
    /// player's name, index, and button properties.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="index"></param>
    /// <param name="button"></param>
    public class Player(string name, int index, IndexButton button) : PlayerModel(name, index, button)
    {
        /// <summary>
        /// constructor for the Player class, which initializes the player's name, index, and button properties.
        /// </summary>
        public Player() : this(string.Empty, 0, new IndexButton()) { }
        /// <summary>
        /// constructor for the Player class, which initializes the player's name and index properties, and 
        /// sets the button property to a new IndexButton object. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        public Player(string name, int index) : this(name, index, new IndexButton()) { }
    }
}
