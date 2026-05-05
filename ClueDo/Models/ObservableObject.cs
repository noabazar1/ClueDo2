using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace ClueDo.Models;
/// <summary>
/// class that implements the INotifyPropertyChanged interface, which is used to notify the UI of changes
/// in the properties of the classes that inherit from it. This class provides a base implementation of the
/// OnPropertyChanged method, which raises the PropertyChanged event when a property value changes. 
/// The classes that inherit from ObservableObject can call the OnPropertyChanged method in their property
/// setters to notify the UI of changes in their properties. 
/// </summary>
public partial class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
