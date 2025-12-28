using ClueDo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.ViewModels
{
    public class SuspectListVM : ObservableObject
    {
        public string Name { get; set; } = string.Empty;
        public ObservableCollection<SuspectItem> Suspects { get; } =
            new ObservableCollection<SuspectItem>
            {
            new SuspectItem { Name = "Miss Scarlet" },
            new SuspectItem { Name = "Colonel Mustard" },
            new SuspectItem { Name = "Mrs. Peacock" }
            };

        public Command<SuspectItem> ToggleMarkCommand { get; }

        public SuspectListVM()
        {
            ToggleMarkCommand = new Command<SuspectItem>(item =>
            {
                item.IsMarked = !item.IsMarked;
                OnPropertyChanged(nameof(Suspects));
            });
        }
    }

}
