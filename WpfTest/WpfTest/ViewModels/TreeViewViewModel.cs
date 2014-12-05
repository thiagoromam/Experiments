using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfTest.Annotations;

namespace WpfTest.ViewModels
{
    public class TreeViewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private object _selected;

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
        public object Selected
        {
            get { return _selected; }
            set
            {
                value = value as string;
                if (value == null || value == _selected) return;
                _selected = value;
                OnPropertyChanged();
            }
        }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}