using CommunityToolkit.Mvvm.Messaging.Messages;

namespace ClueDo.Models
{
    public class AppMessage<T>(T msg) : ValueChangedMessage<T>(msg)
    {

    }
}
