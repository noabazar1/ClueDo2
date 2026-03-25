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
    [ObservableProperty]
    private string timerText = Strings.TimerText;
    [ObservableProperty]
    private string counterText = Strings.CounterText;
    public ChallengePopupVM(Popup popup)
    {
        this.popup = popup;
        WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) =>
        {
            if (m.Value == Keys.FinishedSignal)
            {
                TimerText = Strings.TimerText2;
                popup.Close(false);
            }
            else
            {
                TimeSpan time = TimeSpan.FromMilliseconds(m.Value);
                TimerText = time.ToString(Keys.TimerFormat);
            }
        });
    }
    [RelayCommand]
    private void Click()
    {
        count++;
        CounterText = string.Format(Strings.CounterFormat, count);
        if (count == 20)
            popup.Close();
    }
    public void Cleanup()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}