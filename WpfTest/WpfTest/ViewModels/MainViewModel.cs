using System;
using System.Windows;
using System.Windows.Input;

namespace WpfTest.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            OpenViewCommand = new DelegateCommand(OpenView);
        }

        public ICommand OpenViewCommand { get; private set; }

        private static void OpenView(object viewName)
        {
            var type = Type.GetType("WpfTest.Views." + viewName);

            // ReSharper disable once AssignNullToNotNullAttribute
            var view = (Window)Activator.CreateInstance(type);
            view.Show();
        }
    }
}