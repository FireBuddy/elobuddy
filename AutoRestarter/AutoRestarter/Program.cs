using System;
using System.Diagnostics;

namespace AutoRestarter
{
    class Program
    {
        static int time;
        static string botPath;

        static void Main(string[] args)
        {
            Console.WriteLine("Time for each restart (sec): ");
            time = Convert.ToInt32(Console.ReadLine());

            while (true)
            {
                foreach (Process sysProcess in Process.GetProcesses())
                {
                    if (sysProcess.ProcessName.Contains("bot"))
                    {
                        botPath = sysProcess.MainModule.FileName;
                        sysProcess.Kill();
                        Console.WriteLine("[{0:hh:mm:ss}] closing bot..", DateTime.Now);
                    }
                    else if (sysProcess.ProcessName.Contains("League of Legends"))
                    {
                        sysProcess.Kill();
                        Console.WriteLine("[{0:hh:mm:ss}] closing LoL..", DateTime.Now);
                    }
                }

                Process.Start(botPath);
                Console.WriteLine("[{0:hh:mm:ss}] initializing bot..", DateTime.Now);

                System.Threading.Thread.Sleep(3600000);
            }
        }
    }
}
