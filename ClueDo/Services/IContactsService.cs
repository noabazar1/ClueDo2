using ClueDo.Models;

namespace ClueDo.Services
{
    /// <summary>
    /// interface for a service that allows the user to pick a contact from their device's contact list. 
    /// The service defines a single method, PickContactAsync, which returns a Task that resolves to a 
    /// FriendContact object representing the selected contact.
    /// </summary>
    public interface IContactsService
    {
        Task<FriendContact?> PickContactAsync();
    }
}
