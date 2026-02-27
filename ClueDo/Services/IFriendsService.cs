using ClueDo.Models;

namespace ClueDo.Services
{
    public interface IFriendsService
    {
        Task<List<FriendContact>> GetFriendsAsync();
        Task DeleteFriendAsync(string id);
        Task AddFriendAsync(FriendContact friend);
        Task<bool> IsFriendAsync(string phone);
    }
}
