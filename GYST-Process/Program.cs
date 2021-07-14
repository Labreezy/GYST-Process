using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYST_Process
{
    internal class Program
    {
        static Dictionary<string, TimeSpan> proc_timers = new Dictionary<string, TimeSpan>();
        private Dictionary<string, TimeSpan> browser_title_timers = new Dictionary<string, TimeSpan>();
        static Task _monitorTask;
        static private GYSTMonitor gm;

        
        public static void Main(string[] args)
        {
            gm = new GYSTMonitor();
            gm.windowChanged += OnWindowChange;
            _monitorTask = new Task(gm.monitor);
           _monitorTask.Start();
           Console.WriteLine("GYST started");
           Console.ReadLine();
           gm.cts.Cancel();
           proc_timers.Select(i => $"{i.Key}: {i.Value.ToString()}").ToList().ForEach(Console.WriteLine);

           
        }

        protected static void cancelHandler(object sender, ConsoleCancelEventArgs e)
        {
            gm.cts.Cancel();
            Console.WriteLine("Task Cancelled");
            proc_timers.Select(i => $"{i.Key}: {i.Value.ToString()}").ToList().ForEach(Console.WriteLine);
            
        }

        public static void OnWindowChange(object sender, ProcessUtils.ForegroundWindowChangedEventArgs e)
        {
            
            Console.WriteLine($"Process Change From {e.proc.ProcessName}");
            if (!proc_timers.ContainsKey(e.proc.ProcessName))
            {
                proc_timers[e.proc.ProcessName] = e.timeSpent;
            }
            else
            {
                proc_timers[e.proc.ProcessName] += e.timeSpent;
            }
            Console.WriteLine($"Total time spent on previous: {proc_timers[e.proc.ProcessName].ToString()}");
        }
        
    }
}