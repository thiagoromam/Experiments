using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfTest.Annotations;

namespace WpfTest.ViewModels
{
    public class TreeViewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _selected;

        public TreeViewViewModel()
        {
            SelectCommand = new DelegateCommand(Select);
        }

        public List<object> Favorites
        {
            get
            {
                return new List<object>
                {
                    new
                    {
                        Name = "Games",
                        Items = new[]
                        {
                            "Sonic 3 & Knuckles",
                            "Metroid Zero Mission",
                            "Castlevania Symphony of the Night"
                        }
                    },
                    new
                    {
                        Name = "Musics",
                        Items = new[]
                        {
                            "Megadriver",
                            "Iron Maiden"
                        }
                    }
                };
            }
        }
        public string Selected
        {
            get { return _selected; }
            private set
            {
                if (value == _selected) return;
                _selected = value;
                OnPropertyChanged();
            }
        }
        public ICommand SelectCommand { get; private set; }

        private void Select(object item)
        {
            Selected = "Selected: " + item;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}