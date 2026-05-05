namespace ClueDo.Models
{
    /// <summary>
    /// class representing a contact of the player, used for the "Invite a friend" feature. It contains the
    /// contact's name, phone number and id.
    /// </summary>
    public class FriendContact
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
    }
}
