using System.Windows;
namespace libc.wpf.Win32 {
    public static class SingleInstanceFactory {
        private static SingleInstance singleInstance;
        public static void Initialize(Application application, string uniqueEventName, string uniqueMutexName) {
            singleInstance = new SingleInstance(uniqueEventName, uniqueMutexName, () => {
                var w = application.MainWindow;
                if (w == null)
                    return;
                if (w.WindowState == WindowState.Minimized || w.Visibility == Visibility.Hidden)
                    w.Show();

                // According to some sources these steps gurantee that an app will be brought to foreground.
                w.Activate();
                w.Topmost = true;
                w.Topmost = false;
                w.Focus();
            });
            singleInstance.Run();
        }
        public static void Release() {
            singleInstance?.Dispose();
        }
    }
}