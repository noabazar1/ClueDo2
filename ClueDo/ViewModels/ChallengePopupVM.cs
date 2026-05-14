using ClueDo.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace ClueDo.ViewModels;
/// <summary>
/// class that serves as the view model for a popup challenge in the ClueDo app. It manages the state and 
/// behavior of the popup, including a timer and a counter. The view model listens for messages that 
/// indicate when the timer should be updated or when the challenge is finished, and it updates the UI 
/// accordingly. 
/// </summary>
public partial class ChallengePopupVM : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    private readonly Popup popup;
    private int count = 0;
    [ObservableProperty]
    private string timerText = Strings.TimerText;
    [ObservableProperty]
    private string counterText = Strings.CounterText;
    public ICommand ClickCommand { get; }
    /// <summary>
    /// constructor for the ChallengePopupVM class. It takes a Popup object as a parameter and registers a
    /// message handler using the WeakReferenceMessenger to listen for messages of type AppMessage. When a
    /// message is received, it checks the value of the message. If the value is equal to the FinishedSignal
    /// constant, it updates the TimerText property to indicate that the timer has finished and closes the 
    /// popup. Otherwise, it treats the value as a time in milliseconds, converts it to a TimeSpan object,
    /// formats it as a string using a specified format, and updates the TimerText property with the 
    /// formatted time string. 
    /// </summary>
    /// <param name="popup"></param>
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
        ClickCommand = new Command(Click);
    }
    /// <summary>
    /// method that is called when the user clicks a button in the popup. It increments the count variable,
    /// updates the CounterText property with the new count value formatted as a string, and checks if the 
    /// count has reached 20. If the count is equal to 20, it closes the popup.
    /// </summary>
    private void Click()
    {
        count++;
        CounterText = string.Format(Strings.CounterFormat, count);
        if (count == 20)
            popup.Close();
    }
    /// <summary>
    /// method that is called to clean up resources when the popup is closed. It unregisters all message
    /// handlers.
    /// </summary>
    public void Cleanup()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}