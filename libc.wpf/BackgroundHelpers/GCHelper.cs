using System;
using libc.wpf.Internals;
namespace libwpf.GarbageCollection {
    public static class GCHelper {
        private static WorkerThread gcThread;
        public static void Start() {
            gcThread = new WorkerThread(GC.Collect, 20 * 1000);
            gcThread.Start();
        }
        public static void Stop() {
            gcThread?.Stop();
        }
    }
}