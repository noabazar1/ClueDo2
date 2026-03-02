using CommunityToolkit.Maui.Views;

namespace ClueDo.Services
{
    public class PopupService : IPopupService
    {
        public async Task<object?> ShowAsync(Popup popup)
        {
            return await Shell.Current.CurrentPage.ShowPopupAsync(popup);
        }
    }
}
