using ClueDo.Models;

namespace ClueDo.Services
{
    /// <summary>
    /// class that implements the IContactsService interface, providing functionality to pick a contact
    /// from the user's address book. It uses the Permissions API to request access to the user's contacts
    /// and the Contacts API to allow the user to select a contact.
    /// </summary>
    public class ContactsService : IContactsService
    {
        /// <summary>
        /// method that allows the user to pick a contact from their address book. It first requests 
        /// permission to access the user's contacts, and if granted, it opens the contact picker. If the 
        /// user selects a contact, it creates a FriendContact object containing the contact's display name
        /// and phone number (if available) and returns it. If permission is denied or no contact is 
        /// selected, it returns null.
        /// </summary>
        /// <returns></returns>
        public async Task<FriendContact?> PickContactAsync()
        {
            try
            {
                PermissionStatus permissionStatus =
                    await Permissions.RequestAsync<Permissions.ContactsRead>();
                if (permissionStatus != PermissionStatus.Granted)
                    return null;
                Contact? contact = await Contacts.Default.PickContactAsync();
                if (contact == null)
                    return null;
                return new FriendContact
                {
                    Name = contact.DisplayName,
                    Phone = contact.Phones.FirstOrDefault()?.PhoneNumber
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
