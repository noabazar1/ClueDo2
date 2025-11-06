using Plugin.CloudFirestore;
using System.Text.RegularExpressions;
using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    partial class FbData:FbDataModel
    {
        public override async void CreateUserWithEmailAndPasswordAsync(string email, string password, string name, Action<System.Threading.Tasks.Task> OnComplete)
        {
           await facl.CreateUserWithEmailAndPasswordAsync(email, password, name).ContinueWith(OnComplete);
        }
        public override async void SignInWithEmailAndPasswordAsync(string email, string password, Action<System.Threading.Tasks.Task> OnComplete)
        {
            await facl.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(OnComplete);
        }
        public override string SetDocument(object obj, string collectonName, string id, Action<System.Threading.Tasks.Task> OnComplete)
        {
            IDocumentReference dr = string.IsNullOrEmpty (id) ? fs.Collection(collectonName).Document(): fs.Collection(collectonName).Document(id);
            dr.SetAsync(obj).ContinueWith(OnComplete);
            return dr.Id;
        }

        public override string GetErrorMessage(string errMessage)
        {
            string retMessage = string.Empty;
            //int end, start = errMessage.IndexOf(Keys.MessageKey);
            //if (start > 0)
            //{
            //    end = errMessage.IndexOf(Keys.ErrorsKey, start);

            //    string title = errMessage[(start + Keys.MessageKey.Length)..end]
            //        .Replace(Keys.Apostrophe, string.Empty)
            //        .Replace(Keys.Colon, string.Empty)
            //        .Replace(Keys.Comma, string.Empty)
            //        .Trim();
            //    title = string.Join(Keys.WordsDelimiter, title.Split(Keys.TitleDelimiter));
            //    errMessage = errMessage[(errMessage.IndexOf(Keys.ReasonKey) +
            //        Keys.ReasonKey.Length)..];
            //    errMessage = string.Join(Keys.WordsDelimiter,
            //        Regex.Split(errMessage, Keys.UpperCaseDelimiter)).Trim();
            //    retMessage = title + Keys.NewLine + Keys.ReasonKey +
            //    Keys.WordsDelimiter + errMessage[..^1];
            //}
            //else
            //    retMessage = errMessage;
            return retMessage;
        }
    }
}
