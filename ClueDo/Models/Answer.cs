namespace ClueDo.Models
{
    public class Answer
    {
        private static readonly Random rnd = new Random();
        public string? Room { get; set; }
        public string? Weapon { get; set; }
        public string? Suspect { get; set; }
        public Answer() { } 


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
        public List<string> GetMatchingParts(Accusation suggestion)
        {
            List<string> result = new List<string>();

            if (Room == null || Weapon == null || Suspect == null)
                return result;

            if (Room == suggestion.Room)
                result.Add("Room");

            if (Weapon == suggestion.Weapon)
                result.Add("Weapon");

            if (Suspect == suggestion.Suspect)
                result.Add("Suspect");

            return result;
        }
    }
}
