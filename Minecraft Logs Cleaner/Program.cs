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

        static void DeleteAllFiles(string dir)
        {
            foreach(string file in Directory.GetFiles(dir))
            {
                File.Delete(file);
            }
        }

        static double GetSizeOfFilesInDir_KB(string dir)
        {
            long totalBytes = 0;

            string[] files = Directory.GetFiles(dir);

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                totalBytes += fileInfo.Length;
            }

            return totalBytes / 1024f;
        }

        static double BytesToKB(long bytes)
        {
            return bytes / 1024f;
        }

        // To make the text look a bit prettier, also easier to distinguish what's what
        const string CYAN = "\e[0;36m";
        const string GREEN = "\e[0;32m";
        const string RESET = "\e[0m";

        static void Main(string[] args)
        {

            // Help menu
            if(args.Contains("--help"))
            {
                Console.WriteLine($"{CYAN}--help{RESET} -> Display this help text and list of commands");
                Console.WriteLine($"{CYAN}--no-ask{RESET} -> Clear logs folder without asking first");

                Environment.Exit(0);
            }
            // We need these in order to do anything, so get the directories first
            string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string minecraftLogsDir = $"{homeDir}\\AppData\\Roaming\\.minecraft\\logs";
            string telemetryLogsDir = $"{minecraftLogsDir}\\telemetry";
            

            Console.WriteLine($"Home directory found: {CYAN}{homeDir}{RESET}");
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

                double totalSize_KB = 0;
                totalSize_KB += GetSizeOfFilesInDir_KB(minecraftLogsDir);
                totalSize_KB += GetSizeOfFilesInDir_KB(telemetryLogsDir);

                // Make it more readable; round it down to just one decimal place
                totalSize_KB = Math.Round(totalSize_KB, 1);

                Console.WriteLine($"# Files found in {minecraftLogsDir}: {CYAN}{totalLogs}{RESET}");
                Console.WriteLine($"Total file size: {CYAN}{totalSize_KB} KB{RESET}");

                // Don't mess with the user's files before asking
                if (!args.Contains("--no-ask"))
                {
                    Console.WriteLine("Do you want to clear the logs folder? (Y/N)");
                    string answer = Console.ReadLine().ToLower();

                    if (answer.Equals("y") || answer.Equals("yes"))
                    {
                        DeleteAllFiles(telemetryLogsDir);
                        DeleteAllFiles(minecraftLogsDir);

                        Console.WriteLine($"{CYAN}{totalLogs}{RESET} files were deleted");
                    }

                }
                else
                {
                    // Provide the option to delete the files without asking for input; make automation possible
                    DeleteAllFiles(telemetryLogsDir);
                    DeleteAllFiles(minecraftLogsDir);

                    Console.WriteLine($"{totalLogs} files were deleted");
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
