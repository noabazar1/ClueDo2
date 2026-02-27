using ClueDo.Models;

namespace ClueDo.Services
{
    public class ContactsService : IContactsService
    {
        public async Task<FriendContact?> PickContactAsync()
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
    }
}
