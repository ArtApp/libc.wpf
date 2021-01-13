using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace libc.wpf.Extensions {
    public static class UIExtensions {
        public static void MutexOnUI(this AutoResetEvent are, Dispatcher dispatcher, Action action) {
            are.WaitOne();
            dispatcher.RunOnUIAsync(() => {
                try {
                    action?.Invoke();
                } finally {
                    are.Set();
                }
            });
        }
        public static void RunOnUIAsync(this Action act, Dispatcher dispatcher) {
            dispatcher?.BeginInvoke(DispatcherPriority.Normal, act);
        }
        public static void RunOnUIAsync(this Dispatcher dispatcher, Action act) {
            dispatcher?.BeginInvoke(DispatcherPriority.Normal, act);
        }

        //The action which is invoked in a new thread
        //The action which is invoked in the UI thread after the first one
        //The action which is invoked in the UI thread if the first one rose any exceptions
        public static void RunNewTaskWithUICallback(this Dispatcher dispatcher,
            Action operationAction, Action completionAction, Action<Exception> exceptionAction) {
            Task.Factory.StartNew(() => {
                try {
                    operationAction();
                } catch (Exception e) {
                    dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => exceptionAction(e)));
                }
                dispatcher.BeginInvoke(DispatcherPriority.Normal, completionAction);
            });
        }
    }
}