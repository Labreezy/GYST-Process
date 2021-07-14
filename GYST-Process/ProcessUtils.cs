using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GYST_Process
{
    public class ProcessUtils
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        public static Process GetForegroundProcess()
        {
            uint processID = 0;
            IntPtr hWnd = GetForegroundWindow(); // Get foreground window handle
            uint threadID = GetWindowThreadProcessId(hWnd, out processID); // Get PID from window handle
            Process fgProc = Process.GetProcessById(Convert.ToInt32(processID)); // Get it as a C# obj.
            // NOTE: In some rare cases ProcessID will be NULL. Handle this how you want. 
            return fgProc;
        }

        public static string GetForegroundWindowTitle()
        {
            return ProcessUtils.GetForegroundProcess().MainWindowTitle;
        }
        public class ForegroundWindowChangedEventArgs : EventArgs
        {
            public Process proc { get; set; }
            public TimeSpan timeSpent { get; set; }
        }

        public class WindowTitleChangedEventArgs : EventArgs
        {
            public Process ownerProc { get; set; }
            public TimeSpan procTime { get; set; }
            public string newtitle { get; set; }
            
        }
    }

   
}