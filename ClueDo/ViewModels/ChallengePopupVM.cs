using ClueDo.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ClueDo.ViewModels;

public partial class ChallengePopupVM : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    private readonly Popup popup;
    private int count = 0;
    private bool success = false;

    [ObservableProperty]
    private string timerText = "00:10";

    [ObservableProperty]
    private string counterText = "0 / 20";

    public ChallengePopupVM(Popup popup)
    {
        this.popup = popup;
        WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) =>
        {
            if (m.Value == Keys.FinishedSignal)
            {
                TimerText = "00:00";
                popup.Close(false);
            }
            else
            {
                TimeSpan time = TimeSpan.FromMilliseconds(m.Value);
                TimerText = time.ToString(@"mm\:ss");
            }
        });
    }

    [RelayCommand]
    private void Click()
    {
        count++;
        CounterText = $"{count} / 20";

        if (count == 20)
        {
            popup.Close();
            success = true;
        }
    }

    public void Cleanup()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}