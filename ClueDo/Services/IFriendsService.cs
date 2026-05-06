using ClueDo.Models;

namespace ClueDo.Services
{
    /// <summary>
    /// interface for a service that manages the user's friends list. It defines methods for retrieving the
    /// list of friends, deleting a friend by their ID, adding a new friend, and checking if a phone number
    /// belongs to a friend. 
    /// </summary>
    public interface IFriendsService
    {
        Task<List<FriendContact>> GetFriendsAsync();
        Task DeleteFriendAsync(string id);
        Task AddFriendAsync(FriendContact friend);
        Task<bool> IsFriendAsync(string phone);
    }
}
