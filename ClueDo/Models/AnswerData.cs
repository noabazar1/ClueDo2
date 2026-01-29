namespace ClueDo.Models
{
    public static class AnswerData
    {
        public static readonly List<string> Rooms = new()
        {
            Strings.Kitchen, Strings.Ballroom, Strings.Study, Strings.Library
        };

        public static readonly List<string> Weapons = new()
        {
            Strings.Knife, Strings.Rope, Strings.Candlestick, Strings.LeadPipe, Strings.Revolver, Strings.Wrench
        };

        public static readonly List<string> Suspects = new()
        {
            Strings.MissScarlet, Strings.ColonelMustard, Strings.ProfessorPlum, Strings.MrsWhite, Strings.ReverendGreen, Strings.MrsPeacock 
        };
    }
}
