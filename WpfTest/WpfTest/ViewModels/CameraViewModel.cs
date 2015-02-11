using System;
using System.Windows.Input;
using WpfTest.Components;

namespace WpfTest.ViewModels
{
    public class CameraViewModel : NotificationObject
    {
        private int _imageWidth;

        public CameraViewModel()
        {
            ImageWidth = 1280;
            SetImageWidthCommand = new DelegateCommand(p => ImageWidth = Convert.ToInt32(p), p => Convert.ToInt32(p) != ImageWidth);
        }

        public int ImageWidth
        {
            get { return _imageWidth; }
            private set
            {
                if (value == _imageWidth) return;
                _imageWidth = value;
                OnPropertyChanged();
            }
        }
        public ICommand SetImageWidthCommand { get; private set; }
    }
}