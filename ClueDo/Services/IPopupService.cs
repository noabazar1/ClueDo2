using CommunityToolkit.Maui.Views;

namespace ClueDo.Services
{
    public interface IPopupService
    {
        Task<object?> ShowAsync(Popup popup);
    }
}
