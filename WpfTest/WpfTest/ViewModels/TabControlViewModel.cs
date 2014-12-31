using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTest.Annotations;

namespace WpfTest.ViewModels
{
    public class TabControlViewModel
    {
        public TabControlViewModel()
        {
            Tabs = new ObservableCollection<TabPage>();
            AddTabCommand = new DelegateCommand(o => AddTab());
        }

        public ObservableCollection<TabPage> Tabs { get; private set; }
        public ICommand AddTabCommand { get; private set; }

        private void AddTab()
        {
            foreach (var tab in Tabs)
                tab.IsSelected = false;

            var name = "Tab" + (Tabs.Count + 1);
            Tabs.Add(new TabPage
            {
                Name = name,
                Content = new StackPanel
                {
                    Children =
                    {
                        new TextBlock { Text = name + " content" }
                    }
                },
                IsSelected = true
            });
        }
    }

    public class TabPage : INotifyPropertyChanged
    {
        private bool _isSelected;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public FrameworkElement Content { get; set; }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value.Equals(_isSelected)) return;
                _isSelected = value;
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