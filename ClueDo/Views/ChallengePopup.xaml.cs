using ClueDo.Models;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ClueDo.Views;

public partial class ChallengePopup : Popup, INotifyPropertyChanged
{
    private int count = 0;

    private string timerText = "00:10";
    public string TimerText
    {
        get => timerText;
        set
        {
            if (timerText != value)
            {
                timerText = value;
                OnPropertyChanged();
            }
        }
    }

    public ChallengePopup()
    {
        InitializeComponent();
        BindingContext = this;

        WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) =>
        {
            if (m.Value == Keys.FinishedSignal)
            {
                TimerText = "00:00";
                Close();
                return;
            }

            var time = TimeSpan.FromMilliseconds(m.Value);
            TimerText = time.ToString(@"mm\:ss");
        });
        this.Closed += ChallengePopup_Closed;
    }
    private void ChallengePopup_Closed(object? sender, PopupClosedEventArgs e)
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
    private void OnClicked(object sender, EventArgs e)
    {
        count++;
        lblCounter.Text = $"{count} / 20";

        if (count == 20)
            Close();
    }


    public new event PropertyChangedEventHandler? PropertyChanged;
    protected new void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}