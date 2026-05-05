using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ClueDo.Models
{
    /// <summary>
    /// class used to send messages across the app using the MVVM toolkit's messenger. It inherits from
    /// ValueChangedMessage, which allows it to carry a value of type T. This class can be used to send
    /// various types of messages throughout the application, such as notifications, updates, or commands,
    /// by simply specifying the type of message and the value it carries.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="msg"></param>
    public class AppMessage<T>(T msg) : ValueChangedMessage<T>(msg)
    {
    }
}
