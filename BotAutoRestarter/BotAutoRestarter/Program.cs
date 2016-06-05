using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.Threading.Thread;

namespace BotAutoRestarter
{
    internal static class Program
    {
        private static int time;
        private static string botPath;

        private static void Main()
        {
            Console.WriteLine("Time for each restart (sec): ");
            time = Convert.ToInt32(Console.ReadLine())*1000;

            foreach (
                var fileName in
                    Directory.EnumerateFileSystemEntries(Environment.CurrentDirectory)
                        .Where(file => file.Contains("Bot") || file.Contains("bot") && Path.GetExtension(file) == ".exe")
                )
            {
                botPath = fileName;
                break;
            }

            while (true)
            {
                foreach (var sysProcess in Process.GetProcesses())
                {
                    if (sysProcess.ProcessName.Contains("bot") || sysProcess.ProcessName.Contains("Bot"))
                    {
                        sysProcess.Kill();
                        Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] Closing bot..");
                    }
                    else if (sysProcess.ProcessName.Contains("League of Legends"))
                    {
                        sysProcess.Kill();
                        Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] Closing LoL..");
                    }
                }

                Process.Start(botPath);
                Console.WriteLine($"[{DateTime.Now:hh:mm:ss}] Initializing bot..");

                Sleep(time);
            }
        }
    }
}