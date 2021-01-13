using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
namespace libc.wpf.Win32 {
    public sealed class SingleInstance : IDisposable {
        private readonly Action showMainWindow;
        /// <summary>The event mutex name.</summary>
        private readonly string UniqueEventName;
        /// <summary>The unique mutex name.</summary>
        private readonly string UniqueMutexName;
        /// <summary>The event wait handle.</summary>
        private EventWaitHandle eventWaitHandle;
        /// <summary>The mutex.</summary>
        private Mutex mutex;
        public SingleInstance(string uniqueEventName, string uniqueMutexName, Action showMainWindow) {
            UniqueEventName = uniqueEventName;
            UniqueMutexName = uniqueMutexName;
            this.showMainWindow = showMainWindow;
        }
        public void Dispose() {
            eventWaitHandle?.Dispose();
            mutex?.Dispose();
        }
        public void Run() {
            mutex = new Mutex(true, UniqueMutexName, out var isOwned);
            eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, UniqueEventName);

            // So, R# would not give a warning that this variable is not used.
            GC.KeepAlive(mutex);
            if (isOwned) {
                // Spawn a thread which will be waiting for our event
                var thread = new Thread(
                    () => {
                        while (eventWaitHandle.WaitOne())
                            Application.Current.Dispatcher?.BeginInvoke(new Action(showMainWindow));
                    });

                // It is important mark it as background otherwise it will prevent app from exiting.
                thread.IsBackground = true;
                thread.Start();
                return;
            }

            // Notify other instance so it could bring itself to foreground.
            eventWaitHandle.Set();

            // Terminate this instance.
            Process.GetCurrentProcess().Kill();
        }
    }
}