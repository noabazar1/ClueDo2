using ClueDo.Models;
using Firebase.Database;
using Firebase.Database.Query;
namespace ClueDo.Services
{
    /// <summary>
    /// class that implements the IFriendsService interface and provides methods to manage friends in the
    /// application. It uses Firebase as the backend database to store and retrieve friend information.
    /// </summary>
    public class FriendsService : IFriendsService
    {
        private readonly FirebaseClient _firebase;
        /// <summary>
        /// constructor that initializes the FirebaseClient with the Firebase database link specified in 
        /// the Keys class.
        /// </summary>
        public FriendsService()
        {
            _firebase = new FirebaseClient(Keys.FbLink);
        }
        /// <summary>
        /// method that adds a friend to the Firebase database. It retrieves the user ID from the 
        /// application preferences and then posts the friend information to the Firebase database under 
        /// the user's friends node. After successfully adding the friend, it updates the friend's Id 
        /// property with the key returned from Firebase, which can be used for future reference when 
        /// managing friends (e.g., deleting a friend). This method allows users to add friends to their 
        /// list and store their information securely in the Firebase database.
        /// </summary>
        /// <param name="friend"></param>
        /// <returns></returns>
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
        /// <summary>
        /// method that retrieves the list of friends from the Firebase database. It retrieves the user ID
        /// from the application preferences and then queries the Firebase database for the friends 
        /// associated with that user ID. The method returns a list of FriendContact objects, each 
        /// containing the friend's ID, name, and phone number.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// method that deletes a friend from the Firebase database. It retrieves the user ID from the
        /// application preferences and then deletes the friend information from the Firebase database 
        /// using the friend's ID. This method allows users to remove friends from their list and ensures 
        /// that the corresponding data is also removed from the Firebase database to maintain data 
        /// integrity and privacy.
        /// </summary>
        /// <param name="friendId"></param>
        /// <returns></returns>
        public async Task DeleteFriendAsync(string friendId)
        {
            string userId = Preferences.Get(Keys.UserId, string.Empty);
            await _firebase.Child(Keys.users).Child(userId).Child(Keys.friends).Child(friendId).DeleteAsync();
        }
        /// <summary>
        /// method that checks if a given phone number is already in the user's list of friends. It
        /// retrieves the list of friends from the Firebase database and then checks if any of the friends 
        /// have a phone number that matches the given phone number (after normalizing both phone numbers 
        /// to remove non-digit characters).
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public async Task<bool> IsFriendAsync(string phone)
        {
            List<FriendContact> friends = await GetFriendsAsync();
            return friends.Any(f => Normalize(f.Phone) == Normalize(phone));
        }
        /// <summary>
        /// method that normalizes a phone number by removing all non-digit characters. This is used to 
        /// ensure that phone numbers are compared in a consistent format, regardless of how they are 
        /// entered (e.g., with or without dashes, spaces, parentheses, etc.). The method takes a phone 
        /// number as input and returns a string containing only the digits from the original phone number.
        /// This normalization process helps to avoid issues with different formatting when checking if a 
        /// phone number is already in the user's list of friends.
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
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
