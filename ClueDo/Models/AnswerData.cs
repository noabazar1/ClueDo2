namespace ClueDo.Models
{
    /// <summary>
    /// holds the data for the possible answers in the game.
    /// </summary>
    public static class AnswerData
    {
        public static readonly List<string> Rooms =
        [
            Strings.Kitchen, Strings.Ballroom, Strings.Study, Strings.Library
        ];
        public static readonly List<string> Weapons =
        [
            Strings.Knife, Strings.Rope, Strings.Candlestick, Strings.LeadPipe, Strings.Revolver, Strings.Wrench
        ];
        public static readonly List<string> Suspects =
        [
            Strings.MissScarlet, Strings.ColonelMustard, Strings.ProfessorPlum, Strings.MrsWhite, Strings.ReverendGreen, Strings.MrsPeacock 
        ];
    }
}
