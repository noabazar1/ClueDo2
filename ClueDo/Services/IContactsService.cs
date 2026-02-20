using ClueDo.Models;

namespace ClueDo.Services
{
    public interface IContactsService
    {
        Task<FriendContact?> PickContactAsync();
    }
}
