using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;

namespace ClueDo.Models
{
    public abstract class FbDataModel
    {
        protected FirebaseAuthClient facl;
        protected IFirestore fs;
        protected IWriteBatch? batch;
        public string DisplayName => facl != null && facl.User != null ? facl.User.Info.DisplayName : string.Empty;
        public string UserId => facl != null ? facl.User.Uid : string.Empty;
        public abstract string GetErrorMessage(string errMessage);
        public abstract void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete);
        public abstract string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void UpdateField(string collectonName, string id, string fieldName, object fieldValue, Action<Task> OnComplete);
        public abstract void UpdateFields(string collectonName, string id, Dictionary<string, object> dict, Action<Task> OnComplete);
        public abstract void DeleteDocument(string collectonName, string id, Action<Task> OnComplete);
        public abstract void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete);
        public abstract void GetDocumentsWhereLessThan(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete);
        public abstract IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange);
        public abstract IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange);
        public abstract void StartBatch();
        public abstract void BatchUpdateField(string collectonName, string id, string fName, object fValue);
        public abstract void BatchIncrementField(string collectonName, string id, string fName, long incrementBy);
        public abstract void CommitBatch(Action<System.Threading.Tasks.Task> OnComplete);
        public FbDataModel()
        {
            FirebaseAuthConfig fac = new()
            {
                ApiKey = Keys.FbApiKey,
                AuthDomain = Keys.FbAppDomainKey,
                Providers = [new EmailProvider()]
            };
            facl = new FirebaseAuthClient(fac);
            fs = CrossCloudFirestore.Current.Instance;
        }
    }
}
