using ClueDo.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            FirebaseObject<FriendContact> response = await _firebase
                .Child("users")
                .Child("defaultUser")
                .Child("friends")
                .PostAsync(friend);
            friend.Id = response.Key;
        }
        public async Task<List<FriendContact>> GetFriendsAsync()
        {
            IReadOnlyCollection<FirebaseObject<FriendContact>> result = await _firebase
                .Child("users")
                .Child("defaultUser")
                .Child("friends")
                .OnceAsync<FriendContact>();
            List<FriendContact> friends = result.Select(item => new FriendContact
            {
                Id = item.Key,
                Name = item.Object.Name,
                Phone = item.Object.Phone
            }).ToList();
            return friends;
        }
        public async Task DeleteFriendAsync(string friendId)
        {
            await _firebase.Child("users").Child("defaultUser").Child("friends").Child(friendId).DeleteAsync();
        }
    }
}
