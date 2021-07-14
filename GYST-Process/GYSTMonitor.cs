using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GYST_Process
{
    public class GYSTMonitor
    {
        private Process currentProcess;
        private string currentBraveTitle;
        private bool isBrave;
        public CancellationTokenSource cts;
        private Stopwatch _stopwatch;
        public event System.EventHandler<ProcessUtils.ForegroundWindowChangedEventArgs> windowChanged;
        public GYSTMonitor()
        {
            cts = new CancellationTokenSource();
            currentProcess = ProcessUtils.GetForegroundProcess();
            if (currentProcess.ProcessName.StartsWith("brave"))
            {
                isBrave = true;
                currentBraveTitle = currentProcess.MainWindowTitle;
            }
            else
            {
                isBrave = false;
            }

            _stopwatch = new Stopwatch();
            
        }
        
        public async void monitor()
        {
            _stopwatch.Start();
            try
            {
                while (true)
                {
                    Process newProc = ProcessUtils.GetForegroundProcess();
                    if (newProc.ProcessName != this.currentProcess.ProcessName && newProc.ProcessName != "Idle")
                    {
                        var time = _stopwatch.Elapsed;
                        ProcessUtils.ForegroundWindowChangedEventArgs args =
                            new ProcessUtils.ForegroundWindowChangedEventArgs();
                        args.proc = currentProcess;
                        args.timeSpent = time;
                        EventHandler<ProcessUtils.ForegroundWindowChangedEventArgs> handler = windowChanged;
                        if (handler != null)
                        {
                            handler(this, args);
                        }
                        currentProcess = newProc;
                        _stopwatch.Restart();
                    }
                    Task.Delay(50);

                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}