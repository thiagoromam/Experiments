using System.Collections.ObjectModel;

namespace WpfTest.ViewModels
{
    public class DragAndDropListBoxViewModel
    {
        public DragAndDropListBoxViewModel()
        {
            Games = new ObservableCollection<string>
            {
                "Sonic 3",
                "Sonic & Knuckles",
                "Monster World IV",
                "Lunar: The Silver Star",
                "Lunar: Eternal Blue",
                "Popful Mail",
                "Pier Solar",
                "Crusader of Centy",
                "Beyond Oasis",
                "Battletoads",
                "Ahh! Real Monsters",
            };
        }

        public ObservableCollection<string> Games { get; private set; }
    }
}