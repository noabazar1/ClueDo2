namespace ClueDo.Models
{
    /// <summary>
    /// holds the answer of the game, which is a combination of a room, a weapon and a suspect.
    /// </summary>
    public class Answer
    {
        private static readonly Random rnd = new();
        public string? Room { get; set; }
        public string? Weapon { get; set; }
        public string? Suspect { get; set; }
        /// <summary>
        /// constructor for the Answer class. This constructor is used when creating an Answer object and 
        /// generating a random answer for the game.
        /// </summary>
        public Answer() 
        {
            Room = AnswerData.Rooms[rnd.Next(AnswerData.Rooms.Count)];
            Weapon = AnswerData.Weapons[rnd.Next(AnswerData.Weapons.Count)];
            Suspect = AnswerData.Suspects[rnd.Next(AnswerData.Suspects.Count)];
        }
    }
}
