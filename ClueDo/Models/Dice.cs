namespace ClueDo.Models
{
    /// <summary>
    /// class representing the dice in the game, with two properties for the values of each die.
    /// </summary>
    public class Dice
    {
        public int Die1 { get; set; }
        public int Die2 { get; set; }
        /// <summary>
        /// constructor that initializes the dice with default values of 1 for each die.
        /// </summary>
        public Dice()
        {
            Die1 = 1;
            Die2 = 1;
        }
        /// <summary>
        /// function that simulates rolling the dice by generating random values between 1 and 6 for each die,
        /// using the Random class.
        /// </summary>
        public void RollDice()
        {
            Random rand = new();
            Die1 = rand.Next(1, 7);
            Die2 = rand.Next(1, 7);
        }
    }
}
