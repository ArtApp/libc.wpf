using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace libc.wpf.Helpers {
    public static class TextBoxSelectHelper {
        public static void Start(Dispatcher dispatcher) {
            Task.Run(() => {
                Thread.Sleep(1000);
                dispatcher.RunOnUIAsync(() => {
                    textboxSelect();
                    passwordBoxSelect();
                });
            });
        }
        private static void passwordBoxSelect() {
            var a1 = new Action<object, RoutedEventArgs>((o, e) => {
                (e.OriginalSource as PasswordBox)?.SelectAll();
            });
            var a2 = new Action<object, MouseButtonEventArgs>((o, e) => {
                // Find the PasswordBox
                DependencyObject parent = e.OriginalSource as UIElement;
                while (parent != null && !(parent is PasswordBox))
                    parent = VisualTreeHelper.GetParent(parent);
                if (parent != null) {
                    var passwordBox = (PasswordBox)parent;
                    if (!passwordBox.IsKeyboardFocusWithin) {
                        // If the text box is not yet focused, give it the focus and
                        // stop further processing of this click event.
                        passwordBox.Focus();
                        e.Handled = true;
                    }
                }
            });
            // Select the text in a PasswordBox when it receives focus.
            EventManager.RegisterClassHandler(typeof(PasswordBox),
                UIElement.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(a2));
            EventManager.RegisterClassHandler(typeof(PasswordBox),
                UIElement.GotKeyboardFocusEvent,
                new RoutedEventHandler(a1));
            EventManager.RegisterClassHandler(typeof(PasswordBox),
                Control.MouseDoubleClickEvent,
                new RoutedEventHandler(a1));
        }
        private static void textboxSelect() {
            var a1 = new Action<object, RoutedEventArgs>((o, e) => {
                (e.OriginalSource as TextBox)?.SelectAll();
            });
            var a2 = new Action<object, MouseButtonEventArgs>((o, e) => {
                // Find the TextBox
                DependencyObject parent = e.OriginalSource as UIElement;
                while (parent != null && !(parent is TextBox))
                    parent = VisualTreeHelper.GetParent(parent);
                if (parent != null) {
                    var textBox = (TextBox)parent;
                    if (!textBox.IsKeyboardFocusWithin) {
                        // If the text box is not yet focused, give it the focus and
                        // stop further processing of this click event.
                        textBox.Focus();
                        e.Handled = true;
                    }
                }
            });
            // Select the text in a TextBox when it receives focus.
            EventManager.RegisterClassHandler(typeof(TextBox),
                UIElement.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(a2));
            EventManager.RegisterClassHandler(typeof(TextBox),
                UIElement.GotKeyboardFocusEvent,
                new RoutedEventHandler(a1));
            EventManager.RegisterClassHandler(typeof(TextBox),
                Control.MouseDoubleClickEvent,
                new RoutedEventHandler(a1));
        }
    }
}