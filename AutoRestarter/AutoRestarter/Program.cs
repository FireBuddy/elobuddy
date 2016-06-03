using System;
using System.Diagnostics;

namespace AutoRestarter
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                foreach (Process sysProcess in Process.GetProcesses())
                {
                    if (sysProcess.ProcessName.Contains("ezBot"))
                    {
                        sysProcess.Close();
                        Console.WriteLine("[{0:hh:mm:ss}] closing ezBot..", DateTime.Now);
                    }
                    else if (sysProcess.ProcessName.Contains("League of Legends"))
                    {
                        sysProcess.Close();
                        Console.WriteLine("[{0:hh:mm:ss}] closing LoL..", DateTime.Now);
                    }
                }

                Process.Start("ezBot.exe");
                Console.WriteLine("[{0:hh:mm:ss}] initializing ezBot..", DateTime.Now);

                System.Threading.Thread.Sleep(3600000);
            }
        }
    }
}
