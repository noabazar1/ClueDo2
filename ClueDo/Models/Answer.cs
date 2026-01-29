namespace ClueDo.Models
{
    public class Answer
    {
        public string? Room { get; set; }
        public string? Weapon { get; set; }
        public string? Suspect { get; set; }

        public Answer() { } 

        private static readonly Random rnd = new Random();

        public static Answer Generate()
        {
            return new Answer
            {
                Room = AnswerData.Rooms[rnd.Next(AnswerData.Rooms.Count)],
                Weapon = AnswerData.Weapons[rnd.Next(AnswerData.Weapons.Count)],
                Suspect = AnswerData.Suspects[rnd.Next(AnswerData.Suspects.Count)]
            };
        }
        public bool Matches(Accusation accusation)
        {
            return Room == accusation.Room &&
                   Weapon == accusation.Weapon &&
                   Suspect == accusation.Suspect;
        }

    }
}
