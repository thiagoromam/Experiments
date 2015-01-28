using System.Collections.ObjectModel;

namespace WpfTest.ViewModels
{
    public class DragAndDropListBoxViewModel
    {
        public DragAndDropListBoxViewModel()
        {
            Games = new ObservableCollection<Game>
            {
                new Game("Sonic 3"),
                new Game("Sonic & Knuckles"),
                new Game("Monster World IV"),
                new Game("Lunar: The Silver Star"),
                new Game("Lunar: Eternal Blue"),
                new Game("Popful Mail"),
                new Game("Pier Solar"),
                new Game("Crusader of Centy"),
                new Game("Beyond Oasis"),
                new Game("Battletoads"),
                new Game("Ahh! Real Monsters")
            };
        }

        public ObservableCollection<Game> Games { get; private set; }
    }

    public class Game
    {
        public Game(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}