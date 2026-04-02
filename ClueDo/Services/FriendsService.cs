using ClueDo.Models;
using Firebase.Database;
using Firebase.Database.Query;
namespace ClueDo.Services
{
    public class FriendsService : IFriendsService
    {
        private readonly FirebaseClient _firebase;
        public FriendsService()
        {
            _firebase = new FirebaseClient("https://cluedo-d29ec-default-rtdb.europe-west1.firebasedatabase.app/");
        }
        public async Task AddFriendAsync(FriendContact friend)
        {
            string userId = Preferences.Get(Keys.UserId, string.Empty);
            FirebaseObject<FriendContact> response = await _firebase
                .Child(Keys.users)
                .Child(userId)
                .Child(Keys.friends)
                .PostAsync(friend);
            friend.Id = response.Key;
        }
        public async Task<List<FriendContact>> GetFriendsAsync()
        {
            string userId = Preferences.Get(Keys.UserId, string.Empty);
            IReadOnlyCollection<FirebaseObject<FriendContact>> result = await _firebase
                .Child(Keys.users)
                .Child(userId)
                .Child(Keys.friends)
                .OnceAsync<FriendContact>();
            List<FriendContact> friends = [.. result.Select(item => new FriendContact
            {
                Id = item.Key,
                Name = item.Object.Name,
                Phone = item.Object.Phone
            })];
            return friends;
        }
        public async Task DeleteFriendAsync(string friendId)
        {
            string userId = Preferences.Get(Keys.UserId, string.Empty);
            await _firebase.Child(Keys.users).Child(userId).Child(Keys.friends).Child(friendId).DeleteAsync();
        }
        public async Task<bool> IsFriendAsync(string phone)
        {
            List<FriendContact> friends = await GetFriendsAsync();
            return friends.Any(f => Normalize(f.Phone) == Normalize(phone));
        }
        private static string Normalize(string? phone)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(phone))
            {
                result = new string([.. phone.Where(char.IsDigit)]);
            }
            return result;
        }
    }
}
