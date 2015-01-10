using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows;

namespace SmartPrinter.UI.Behaviors
{
    public class DraggableWindowBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += (s, e) => AttachBehavior();
        }

        protected override void OnDetaching()
        {
            DetachBehavior();

            base.OnDetaching();
        }

        private void AttachBehavior()
        {
            Window source = AssociatedObject;

            source.BorderThickness = new Thickness(0);

            source.ResizeMode = ResizeMode.NoResize;

            source.WindowStyle = WindowStyle.None;

            source.MouseLeftButtonDown += OnWindowMouseLeftButtonDown;
        }

        private void DetachBehavior()
        {
            Window source = AssociatedObject;

            if (source == null)
            {
                return;
            }

            source.MouseLeftButtonDown -= OnWindowMouseLeftButtonDown;
        }

        private void OnWindowMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            AssociatedObject.DragMove();
        }
    }
}