using Plugin.CloudFirestore;
using System.Text.RegularExpressions;
using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    /// <summary>
    /// class that implements the methods of the FbDataModel class, which are used to interact with the 
    /// Firebase Firestore database. This class is used in the GamesModel and PlayersModel classes to 
    /// perform database operations such as creating users, signing in, setting documents, getting error
    /// messages, adding snapshot listeners, getting documents based on conditions, updating fields, 
    /// deleting documents, and managing batch operations. The implementation of these methods will depend 
    /// on the specific requirements of the game, and how the data is structured in the database. 
    /// This class serves as a bridge between the game logic and the database operations, allowing the 
    /// game to interact with the database in a consistent and efficient manner.
    /// </summary>
    public partial class FbData : FbDataModel
    {
        /// <summary>
        /// method to create a new user account in the authentication system, and to update the user's 
        /// information and status during the registration process. This method takes the user's email,
        /// password, and name as parameters and uses the Firebase Authentication API to create a new user
        /// account. The method also takes an Action delegate as a parameter, which is called when the 
        /// registration process is complete. The implementation of this method will depend on how the user 
        /// information is stored in the database, and how the user's status is updated during the
        /// registration process.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="name"></param>
        /// <param name="OnComplete"></param>
        public override async void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.CreateUserWithEmailAndPasswordAsync(email, password, name).ContinueWith(OnComplete);
        }
        /// <summary>
        /// method to authenticate the user with the provided email and password, and to update the user's 
        /// information and status during the login process. This method takes the user's email and password
        /// as parameters and uses the Firebase Authentication API to sign in the user. The method also 
        /// takes an Action delegate as a parameter, which is called when the login process is complete. 
        /// The implementation of this method will depend on how the user information is stored in the 
        /// database, and how the user's status is updated during the login process.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="OnComplete"></param>
        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
        }
        /// <summary>
        /// method to set a document in the Firestore database. This method takes an object to be stored,
        /// the name of the collection, an optional document ID, and an Action delegate as parameters. If
        /// the document ID is not provided, a new document will be created with a generated ID. The method
        /// uses the Firebase Firestore API to set the document in the specified collection, and it calls
        /// the Action delegate when the operation is complete. 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="OnComplete"></param>
        /// <returns></returns>
        public override string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete)
        {
            IDocumentReference dr = string.IsNullOrEmpty(id) ? fs.Collection(collectonName).Document() : fs.Collection(collectonName).Document(id);
            dr.SetAsync(obj).ContinueWith(OnComplete);
            return dr.Id;
        }
        /// <summary>
        /// method to extract a user-friendly error message from the error message returned by the Firebase
        /// API. This method takes the error message as a parameter and uses string manipulation and
        /// regular expressions to extract the relevant information and format it in a more readable way.
        /// The implementation of this method will depend on the specific format of the error messages 
        /// returned by the Firebase API, and how the relevant information is structured in those messages.
        /// </summary>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public override string GetErrorMessage(string errMessage)
        {
            string retMessage;
            int end, start = errMessage.IndexOf(Keys.MessageKey);
            if (start > 0)
            {
                end = errMessage.IndexOf(Keys.ErrorsKey, start);

                string title = errMessage[(start + Keys.MessageKey.Length)..end]
                    .Replace(Keys.Apostrophe, string.Empty)
                    .Replace(Keys.Colon, string.Empty)
                    .Replace(Keys.Comma, string.Empty)
                    .Trim();
                title = string.Join(Keys.WordsDelimiter, title.Split(Keys.TitleDelimiter));
                errMessage = errMessage[(errMessage.IndexOf(Keys.ReasonKey) +
                    Keys.ReasonKey.Length)..];
                errMessage = string.Join(Keys.WordsDelimiter,
                    Regex.Split(errMessage, Keys.UpperCaseDelimiter)).Trim();
                retMessage = title + Keys.NewLine + Keys.ReasonKey +
                Keys.WordsDelimiter + errMessage[..^1];
            }
            else
                retMessage = errMessage;
            return retMessage;
        }
        /// <summary>
        /// method to add a snapshot listener to a collection or a document in the Firestore database. 
        /// This method takes the name of the collection, an optional document ID, and a snapshot handler
        /// as parameters. If the document ID is not provided, the snapshot listener will be added to the 
        /// entire collection, and it will be triggered whenever there is a change in any document within 
        /// that collection. If the document ID is provided, the snapshot listener will be added to the 
        /// specific document, and it will be triggered whenever there is a change in that document. 
        /// The method uses the Firebase Firestore API to add the snapshot listener to the specified 
        /// collection or document, and it returns an IListenerRegistration object that can be used to 
        /// remove the listener when it is no longer needed. 
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="OnChange"></param>
        /// <returns></returns>
        public override IListenerRegistration AddSnapshotListener(string collectonName, Plugin.CloudFirestore.QuerySnapshotHandler OnChange)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            return cr.AddSnapshotListener(OnChange);
        }
        /// <summary>
        /// method to add a snapshot listener to a specific document in the Firestore database. This method
        /// takes the name of the collection, the document ID, and a snapshot handler as parameters. The 
        /// snapshot listener will be triggered whenever there is a change in the specified document. The
        /// method uses the Firebase Firestore API to add the snapshot listener to the specified document,
        /// and it returns an IListenerRegistration object that can be used to remove the listener when it
        /// is no longer needed. 
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="OnChange"></param>
        /// <returns></returns>
        public override IListenerRegistration AddSnapshotListener(string collectonName, string id, Plugin.CloudFirestore.DocumentSnapshotHandler OnChange)
        {
            IDocumentReference cr = fs.Collection(collectonName).Document(id);
            return cr.AddSnapshotListener(OnChange);
        }
        /// <summary>
        /// method to get documents from a collection in the Firestore database based on a specific 
        /// condition. This method takes the name of the collection, the field name, the field value, and
        /// an Action delegate as parameters. The method uses the Firebase Firestore API to query the 
        /// specified collection for documents where the value of the specified field is equal to the 
        /// provided field value. The method retrieves the matching documents and calls the Action delegate
        /// with the query snapshot containing the results. 
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="fName"></param>
        /// <param name="fValue"></param>
        /// <param name="OnComplete"></param>
        public override async void GetDocumentsWhereEqualTo(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereEqualsTo(fName, fValue).GetAsync();
            OnComplete(qs);
        }
        /// <summary>
        /// method to get documents from a collection in the Firestore database based on a specific 
        /// condition. This method takes the name of the collection, the field name, the field value, and
        /// an Action delegate as parameters. The method uses the Firebase Firestore API to query the
        /// specified collection for documents where the value of the specified field is less than the 
        /// provided field value. The method retrieves the matching documents and calls the Action delegate
        /// with the query snapshot containing the results.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="fName"></param>
        /// <param name="fValue"></param>
        /// <param name="OnComplete"></param>
        public override async void GetDocumentsWhereLessThan(string collectonName, string fName, object fValue, Action<IQuerySnapshot> OnComplete)
        {
            ICollectionReference cr = fs.Collection(collectonName);
            IQuerySnapshot qs = await cr.WhereLessThan(fName, fValue).GetAsync();
            OnComplete(qs);
        }
        /// <summary>
        /// method to update specific fields in a document in the Firestore database. This method takes the
        /// name of the collection, the document ID, a dictionary containing the field names and their new 
        /// values, and an Action delegate as parameters. The method uses the Firebase Firestore API to
        /// update the specified fields in the document with the provided values. The method calls the 
        /// Action delegate when the update operation is complete.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="dict"></param>
        /// <param name="OnComplete"></param>
        public override async void UpdateFields(string collectonName, string id, Dictionary<string, object> dict, Action<Task> OnComplete)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            await dr.UpdateAsync(dict).ContinueWith(OnComplete);
        }
        /// <summary>
        /// method to delete a document from the Firestore database. This method takes the name of the 
        /// collection, the document ID, and an Action delegate as parameters. The method uses the Firebase
        /// Firestore API to delete the specified document from the collection. The method calls the Action
        /// delegate when the delete operation is complete.
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="OnComplete"></param>
        public override async void DeleteDocument(string collectonName, string id, Action<Task> OnComplete)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            await dr.DeleteAsync().ContinueWith(OnComplete);
        }
        /// <summary>
        /// method to update a specific field in a document in the Firestore database. This method takes 
        /// the name of the collection, the document ID, the field name, the new field value, and an Action
        /// delegate as parameters. The method uses the Firebase Firestore API to update the specified 
        /// field in the document with the provided value. The method calls the Action delegate when the 
        /// update operation is complete. This method is used to update a single field in a document, and 
        /// it can be called multiple times to update different fields in the same document. 
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <param name="OnComplete"></param>
        public override async void UpdateField(string collectonName, string id, string fieldName, object fieldValue, Action<Task> OnComplete)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            await dr.UpdateAsync(fieldName, fieldValue).ContinueWith(OnComplete);
        }
        /// <summary>
        /// method to start a batch operation in the Firestore database. This method initializes a batch
        /// object that can be used to perform multiple write operations (such as updates or deletes) in a 
        /// single atomic operation. The batch object allows you to group multiple operations together, and
        /// it will ensure that either all of the operations succeed or none of them are applied to the 
        /// database. This method is called before performing any batch updates, and it prepares the batch
        /// object for use in subsequent batch update methods.
        /// </summary>
        public override void StartBatch()
        {
            batch = fs.Batch();
        }
        /// <summary>
        /// method to update a specific field in a document as part of a batch operation in the Firestore
        /// database. This method takes the name of the collection, the document ID, the field name, and
        /// the new field value as parameters. The method uses the batch object to queue an update
        /// operation for the specified field in the document with the provided value. This method does not
        /// execute the update immediately; instead it adds the update operation to the batch, which will be
        /// executed when the CommitBatch method is called. 
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="fName"></param>
        /// <param name="fValue"></param>
        public override void BatchUpdateField(string collectonName, string id, string fName, object fValue)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            batch?.Update(dr, fName, fValue);
        }
        /// <summary>
        /// method to increment a specific field in a document as part of a batch operation in the Firestore
        /// database. This method takes the name of the collection, the document ID, the field name, and 
        /// the increment value as parameters. The method uses the batch object to queue an update operation
        /// that increments the specified field in the document by the provided increment value. This 
        /// method does not execute the update immediately; instead it adds the increment operation to the
        /// batch, which will be executed when the CommitBatch method is called. 
        /// </summary>
        /// <param name="collectonName"></param>
        /// <param name="id"></param>
        /// <param name="fName"></param>
        /// <param name="incrementBy"></param>
        public override void BatchIncrementField(string collectonName, string id, string fName, long incrementBy)
        {
            IDocumentReference dr = fs.Collection(collectonName).Document(id);
            batch?.Update(dr, fName, FieldValue.Increment(incrementBy));
        }
        /// <summary>
        /// method to commit the batch operation in the Firestore database. This method executes all of the
        /// queued write operations in the batch atomically. If any of the operations fail, the entire batch
        /// is rolled back and none of the changes are applied to the database. This method is called after
        /// all batch update methods have been called to finalize the batch operation.
        /// </summary>
        /// <param name="OnComplete"></param>
        public override void CommitBatch(Action<System.Threading.Tasks.Task> OnComplete)
        {
            batch?.CommitAsync().ContinueWith(OnComplete);
        }
    }
}