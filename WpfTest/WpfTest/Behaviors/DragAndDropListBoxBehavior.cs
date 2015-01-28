using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfTest.Helpers;

namespace WpfTest.Behaviors
{
    // http://www.hardcodet.net/2009/03/moving-data-grid-rows-using-drag-and-drop
    public class DragAndDropListBoxBehavior : Behavior<ListBox>
    {
        public static readonly DependencyProperty DraggedItemProperty;
        private readonly Popup _popup;
        private Panel _panel;
        private bool _isDragging;

        static DragAndDropListBoxBehavior()
        {
            DraggedItemProperty = DependencyProperty.Register(
                "DraggedItem",
                typeof(object),
                typeof(DragAndDropListBoxBehavior)
            );
        }
        public DragAndDropListBoxBehavior()
        {
            var textBlock = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(8, 0, 0, 0)
            };

            _popup = new Popup
            {
                IsHitTestVisible = false,
                Placement = PlacementMode.RelativePoint,
                AllowsTransparency = true,
                Child = new Border
                {
                    BorderBrush = Brushes.LightSteelBlue,
                    BorderThickness = new Thickness(2),
                    Background = Brushes.White,
                    Opacity = 0.75,
                    Child = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(4, 3, 8, 3),
                        Children =
                        {
                            new Image
                            {
                                Source = new BitmapImage(new Uri("/Images/DragInsert.png", UriKind.Relative)),
                                Width = 16,
                                Height = 16
                            },
                            textBlock
                        }
                    }
                }
            };

            _popup.SetBinding(Popup.PlacementTargetProperty, new Binding { Source = this });
            textBlock.SetBinding(TextBlock.TextProperty, new Binding("DraggedItem") { Source = this });
        }
        
        public object DraggedItem
        {
            get { return GetValue(DraggedItemProperty); }
            set { SetValue(DraggedItemProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            // ReSharper disable once PossibleNullReferenceException
            _panel = (Panel)Window.GetWindow(AssociatedObject).Content;
            _panel.Children.Add(_popup);

            AssociatedObject.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            AssociatedObject.MouseMove += OnMouseMove;
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            
            _panel.Children.Remove(_popup);

            AssociatedObject.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
            AssociatedObject.MouseMove -= OnMouseMove;
            AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var row = UiHelpers.TryFindFromPoint<ListBoxItem>((UIElement)sender, e.GetPosition(AssociatedObject));
            if (row == null) return;

            _isDragging = true;
            DraggedItem = row.DataContext;
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging || e.LeftButton != MouseButtonState.Pressed) return;

            if (!_popup.IsOpen)
                _popup.IsOpen = true;

            var popupSize = new Size(_popup.ActualWidth, _popup.ActualHeight);
            _popup.PlacementRectangle = new Rect(e.GetPosition(AssociatedObject), popupSize);

            var position = e.GetPosition(AssociatedObject);
            var row = UiHelpers.TryFindFromPoint<DataGridRow>(AssociatedObject, position);
            if (row != null)
                AssociatedObject.SelectedItem = row.Item;
        }
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isDragging)
                return;

            var collection = (IList)AssociatedObject.ItemsSource;
            var targetItem = AssociatedObject.SelectedItem;

            if (targetItem == null || !ReferenceEquals(DraggedItem, targetItem))
            {
                collection.Remove(DraggedItem);
                var targetIndex = collection.IndexOf(targetItem);
                collection.Insert(targetIndex, DraggedItem);

                AssociatedObject.SelectedItem = DraggedItem;
            }

            _isDragging = false;
            _popup.IsOpen = false;
        }
    }
}