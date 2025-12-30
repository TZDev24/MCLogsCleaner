using System.Runtime.InteropServices;

namespace Minecraft_Logs_Cleaner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // We need these in order to do anything, so get the directories first
            string home_dir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string minecraft_dir = $"{home_dir}\\AppData\\Roaming\\.minecraft";

            Console.WriteLine($"Home directory: {home_dir}");
            // Check if the minecraft_dir exists
            if (Directory.Exists(minecraft_dir))
            {
                Console.WriteLine($"Minecraft directory located: ({minecraft_dir})");
            }
            else
            {
                Console.WriteLine("Minecraft directory not found, exiting");
                Environment.Exit(0);
            }
        }
    }
}
