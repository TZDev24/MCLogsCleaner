using System.Drawing;
using System.Runtime.InteropServices;

namespace Minecraft_Logs_Cleaner
{
    internal class Program
    {

        static int GetNumFiles(string dir)
        {
            return Directory.GetFiles(dir).Length;
        }

        static long GetFileSize(string file)
        {
            FileInfo fileInfo = new FileInfo(file);
            return fileInfo.Length;
        }

        // To make the text look a bit prettier, also easier to distinguish what's what
        const string CYAN = "\e[0;36m";
        const string GREEN = "\e[0;32m";
        const string RESET = "\e[0m";

        static void Main(string[] args)
        {
            // We need these in order to do anything, so get the directories first
            string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string minecraftLogsDir = $"{homeDir}\\AppData\\Roaming\\.minecraft\\logs";
            

            Console.WriteLine($"Home directory: {CYAN}{homeDir}{RESET}");
            // Check if the minecraft_dir exists
            if (Directory.Exists(minecraftLogsDir))
            {
                Console.WriteLine($"Minecraft directory located: {CYAN}{minecraftLogsDir}{RESET}");

                int numLogs = GetNumFiles(minecraftLogsDir);
                // There's another directory in there with more logs, count those too.
                // Count the logs in logs and \telemetry, but don't count the telemetry folder itself
                int numTelemetryLogs = GetNumFiles($"{minecraftLogsDir}\\telemetry");
                int totalLogs = numLogs + numTelemetryLogs;

                if (totalLogs == 0)
                {
                    Console.WriteLine($"No log files were found. {GREEN}Logs directory is Empty!{RESET} Exiting");
                    Environment.Exit(0);
                }

                // Start collecting the file sizes of every file
                long totalFileSize_bytes = 0L;
                string[] logFiles = Directory.GetFiles(minecraftLogsDir);
                string[] telemetryLogFiles = Directory.GetFiles($"{minecraftLogsDir}\\telemetry");

                foreach(string file in logFiles)
                {
                    totalFileSize_bytes += GetFileSize(file);
                }

                foreach (string file in telemetryLogFiles)
                {
                    totalFileSize_bytes += GetFileSize(file);
                }

                double totalSize_KB = totalFileSize_bytes / 1024f;
                totalSize_KB = Math.Round(totalSize_KB, 1);

                Console.WriteLine($"# Files found in {minecraftLogsDir}: {CYAN}{totalLogs}{RESET}");
                Console.WriteLine($"Total file size: {CYAN}{totalSize_KB} KB{RESET}");

                Console.WriteLine("Do you want to clear the logs folder? (Y/N)");
                string answer = Console.ReadLine().ToLower();

                if (answer.Equals("y") || answer.Equals("yes"))
                {
                    foreach (string file in telemetryLogFiles)
                    {
                        File.Delete(file);
                    }

                    foreach (string file in logFiles)
                    {
                        File.Delete(file);
                    }
                }
            }
            else
            {
                Console.WriteLine("Minecraft directory not found (not installed?), exiting");
                Environment.Exit(0);
            }
        }
    }
}
