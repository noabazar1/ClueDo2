using Firebase.Auth;
using Firebase.Auth.Providers;
using Plugin.CloudFirestore;

namespace ClueDo.Models
{
    /// <summary>
    /// class that abstracts the FirebaseAuthClient and IFirestore instances and provides methods for
    /// authentication and database operations. It also includes a method for parsing error messages from
    /// Firebase exceptions to make them more user-friendly.
    /// </summary>
    public abstract class FbDataModel
    {
        protected FirebaseAuthClient facl;
        protected IFirestore fs;
        protected IWriteBatch? batch;
        public string DisplayName => facl != null && facl.User != null ? facl.User.Info.DisplayName : string.Empty;
        public string UserId => facl != null ? facl.User.Uid : string.Empty;
        /// <summary>
        /// abstract method that takes an error message string as input and returns a more user-friendly error
        /// message. The implementation of this method is expected to parse the original error message,
        /// extract relevant information, and format it in a way that is easier for users to understand.
        /// </summary>
        /// <param name="errMessage"></param>
        public abstract string GetErrorMessage(string errMessage);
        /// <summary>
        /// abstract method that takes an email, password, name, and a callback function as parameters.
        /// The method is expected to create a new user account using the provided email and password, and
        /// then call the callback function with the result of the operation. 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="OnComplete"></param>
        public abstract void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete);
        /// <summary>
        /// abstract method that takes an email, password, and a callback function as parameters. The method
        /// is expected to sign in an existing user using the provided email and password, and then call the
        /// callback function with the result of the operation.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="OnComplete"></param>
        public abstract void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete);
        /// <summary>
        /// abstract method that takes an object, collection name, document ID, and a callback function as
        /// parameters. The method is expected to set a document in the specified collection with the provided
        /// data. If the operation is successful, the callback function should be called with the result of
        /// the operation.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="OnComplete"></param>
        /// <returns></returns>
        public abstract string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete);
        /// <summary>
        /// abstract method that takes a collection name, document ID, field name, field value, and a callback
        /// function as parameters. The method is expected to update a specific field in a document within the
        /// specified collection. If the operation is successful, the callback function should be called with
        /// the result of the operation. 
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="OnComplete"></param>
        public abstract void UpdateField(string collectonName, string id, string fieldName, object fieldValue, Action<Task> OnComplete);
        /// <summary>
        /// abstract method that takes a collection name, document ID, a dictionary of field names and values,
        /// and a callback function as parameters. The method is expected to update multiple fields in a
        /// document within the specified collection using the provided dictionary. If the operation is
        /// successful, the callback function should be called with the result of the operation.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="dict"></param>
        /// <param name="OnComplete"></param>
        public abstract void UpdateFields(string collectonName, string id, Dictionary<string, object> dict, Action<Task> OnComplete);
        /// <summary>
        /// abstract method that takes a collection name, document ID, and a callback function as parameters.
        /// The method is expected to delete a document from the specified collection using the provided
        /// document ID. If the operation is successful, the callback function should be called with the
        /// result of the operation.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="OnComplete"></param>
        public abstract void DeleteDocument(string collectonName, string id, Action<Task> OnComplete);
        /// <summary>
        /// abstract method that takes a collection name, field name, field value, and a callback function as
        /// parameters. The method is expected to retrieve documents from the specified collection where the
        /// value of a specific field matches the provided value. If the operation is successful, the callback
        /// function should be called with the result of the operation, which is expected to be an
        /// IQuerySnapshot containing the matching documents.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="fName"></param>
        /// <param name="fValue"></param>
        /// <param name="OnComplete"></param>
        public abstract void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete);
        /// <summary>
        /// abstract method that takes a collection name, field name, field value, and a callback function as
        /// parameters. The method is expected to retrieve documents from the specified collection where the
        /// value of a specific field is less than the provided value. If the operation is successful, the
        /// callback function should be called with the result of the operation, which is expected to be an
        /// IQuerySnapshot containing the matching documents.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="fName"></param>
        /// <param name="fValue"></param>
        /// <param name="OnComplete"></param>
        public abstract void GetDocumentsWhereLessThan(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete);
        /// <summary>
        /// abstract method that takes a collection name and a callback function as parameters. The method is
        /// expected to add a snapshot listener to the specified collection. The snapshot listener should
        /// trigger the provided callback function whenever there are changes to the documents in the
        /// collection. The callback function is expected to handle the changes and update the application
        /// state accordingly. The method should return an IListenerRegistration object that can be used to
        /// remove the listener when it is no longer needed.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="OnChange"></param>
        public abstract IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange);
        /// <summary>
        /// abstract method that takes a collection name, document ID, and a callback function as parameters.
        /// The method is expected to add a snapshot listener to a specific document within the specified
        /// collection. The snapshot listener should trigger the provided callback function whenever there are
        /// changes to the document. The callback function is expected to handle the changes and update the
        /// application state accordingly. The method should return an IListenerRegistration object that can
        /// be used to remove the listener when it is no longer needed.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="OnChange"></param>
        public abstract IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange);
        /// <summary>
        /// abstract method that takes a collection name, document ID, field name, field value, and a callback
        /// function as parameters. The method is expected to update a specific field in a document within the
        /// specified collection. If the operation is successful, the callback function should be called with
        /// the result of the operation. This method is intended to be used for batch updates, allowing
        /// multiple field updates to be performed in a single batch operation. The implementation of this
        /// method should ensure that the updates are added to the batch and that the batch is committed
        /// properly when the CommitBatch method is called.
        /// </summary>
        public abstract void StartBatch();
        /// <summary>
        /// abstract method that takes a collection name, document ID, field name, and field value as
        /// parameters. The method is expected to add an update operation to a batch for updating a specific
        /// field in a document within the specified collection. This method is intended to be used in
        /// conjunction with the StartBatch and CommitBatch methods to perform multiple updates in a single
        /// batch operation. The implementation of this method should ensure that the update operation is
        /// added to the batch and that the batch is committed properly when the CommitBatch method is called.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="fName"></param>
        /// <param name="fValue"></param>
        public abstract void BatchUpdateField(string collectonName, string id, string fName, object fValue);
        /// <summary>
        /// abstract method that takes a collection name, document ID, field name, and an increment value as 
        /// parameters. The method is expected to add an increment operation to a batch for incrementing a
        /// specific field in a document within the specified collection. This method is intended to be used
        /// in conjunction with the StartBatch and CommitBatch methods to perform multiple updates in a single
        /// batch operation. The implementation of this method should ensure that the increment operation is
        /// added to the batch and that the batch is committed properly when the CommitBatch method is called.
        /// The increment operation should increase the value of the specified field by the provided increment
        /// value when the batch is committed.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="fName"></param>
        /// <param name="incrementBy"></param>
        public abstract void BatchIncrementField(string collectonName, string id, string fName, long incrementBy);
        /// <summary>
        /// abstract method that takes a callback function as a parameter. The method is expected to commit
        /// the current batch of operations that have been added using the BatchUpdateField and
        /// BatchIncrementField methods. If the operation is successful, the callback function should be
        /// called with the result of the operation. The implementation of this method should ensure that the
        /// batch is committed properly and that any errors are handled appropriately, with the callback
        /// function being called with the result of the operation regardless of success or failure. This
        /// method is essential for ensuring that all batch operations are executed together, providing
        /// atomicity and consistency in the database updates.
        /// </summary>
        /// <param name="OnComplete"></param>
        public abstract void CommitBatch(Action<System.Threading.Tasks.Task> OnComplete);
        /// <summary>
        /// constructor for the FbDataModel class. It initializes the FirebaseAuthClient with the necessary 
        /// configuration for authentication, including the API key, authentication domain, and providers. It
        /// also initializes the Firestore instance using the CrossCloudFirestore plugin. This setup allows
        /// the FbDataModel class to handle authentication and database operations using Firebase services.
        /// </summary>
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
