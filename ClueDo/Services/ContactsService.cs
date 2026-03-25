using ClueDo.Models;

namespace ClueDo.Services
{
    public class ContactsService : IContactsService
    {
        public async Task<FriendContact?> PickContactAsync()
        {
            FriendContact? result = null;
            PermissionStatus permissionStatus = await Permissions.RequestAsync<Permissions.ContactsRead>();
            if (permissionStatus == PermissionStatus.Granted)
            {
                Contact? contact = await Contacts.Default.PickContactAsync();
                if (contact != null)
                {
                    result = new FriendContact
                    {
                        Name = contact.DisplayName,
                        Phone = contact.Phones.FirstOrDefault()?.PhoneNumber
                    };
                }
            }
            return result;
        }
    }
}
