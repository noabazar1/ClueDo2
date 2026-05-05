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
        /// constructor for the Answer class, which initializes the properties to null.
        /// This constructor is used when creating an Answer object without specifying the values,
        /// and the values will be set later using the Generate method.
        /// </summary>
        public Answer() { }
        /// <summary>
        /// method to generate a random answer for the game, by randomly selecting a room, a weapon and
        /// a suspect from the predefined lists in the AnswerData class.
        /// </summary>
        /// <returns>
        /// a random Answer object
        /// </returns>
        public static Answer Generate()
        {
            return new Answer
            {
                Room = AnswerData.Rooms[rnd.Next(AnswerData.Rooms.Count)],
                Weapon = AnswerData.Weapons[rnd.Next(AnswerData.Weapons.Count)],
                Suspect = AnswerData.Suspects[rnd.Next(AnswerData.Suspects.Count)]
            };
        }
    }
}
