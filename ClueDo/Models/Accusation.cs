namespace ClueDo.Models
{
    /// <summary>
    /// holds the data of an accusation, which is a guess of the player about the room, weapon and suspect.
    /// </summary>
    public class Accusation
    {
        public string Room { get; set; } = string.Empty;
        public string Weapon { get; set; } = string.Empty;
        public string Suspect { get; set; } = string.Empty;
    }
}
