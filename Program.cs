using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace ModController
{
    class Program
    {
        static void Main()
        {
            ConsoleUI.ShowBanner();

            var config = ConfigLoader.Load("modConfig.json");
            if (config == null || config.ModPaths == null || config.ModPaths.Count == 0)
            {
                ConsoleUI.ShowError("modConfig.json could not be loaded or is empty.");
                return;
            }

            ConsoleUI.ShowInfo($"Current mod status: {config.ModStatus ?? "Unknown"}");

            bool exitRequested = false;
            while (!exitRequested)
            {
                ConsoleUI.ShowMenu();
                var key = Console.ReadKey(true).Key;

                Console.Clear(); // 🧼 Refresh screen after each action

                switch (key)
                {
                    case ConsoleKey.E:
                        ModManager.EnableMods(config);
                        break;
                    case ConsoleKey.D:
                        ModManager.DisableMods(config);
                        break;
                    case ConsoleKey.Q:
                        ConsoleUI.ShowExit();
                        exitRequested = true;
                        break;
                    default:
                        ConsoleUI.ShowWarning("Invalid selection. Please try again.");
                        break;
                }
            }
        }
    }

    class Config
    {
        public List<string> ModPaths { get; set; }
        public string ModStatus { get; set; }
    }

    static class ConfigLoader
    {
        public static Config Load(string path)
        {
            try
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<Config>(json);
            }
            catch (Exception ex)
            {
                ConsoleUI.ShowError($"Failed to read config: {ex.Message}");
                return null;
            }
        }

        public static void Save(Config config, string path)
        {
            try
            {
                var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                ConsoleUI.ShowError($"Failed to save config: {ex.Message}");
            }
        }
    }

    static class ModManager
    {
        private static readonly string BackupFolder = "ModsBackup";

        public static void DisableMods(Config config)
        {
            ConsoleUI.ShowInfo("Disabling mods...");
            Directory.CreateDirectory(BackupFolder);

            foreach (var path in config.ModPaths)
            {
                if (File.Exists(path))
                {
                    var dest = Path.Combine(BackupFolder, Path.GetFileName(path));
                    File.Move(path, dest, true);
                    ConsoleUI.ShowSuccess($"Moved: {path} → {dest}");
                }
                else if (Directory.Exists(path))
                {
                    var dest = Path.Combine(BackupFolder, new DirectoryInfo(path).Name);
                    Directory.Move(path, dest);
                    ConsoleUI.ShowSuccess($"Folder moved: {path} → {dest}");
                }
                else
                {
                    ConsoleUI.ShowWarning($"Not found: {path}");
                }
            }

            config.ModStatus = "Disabled";
            ConfigLoader.Save(config, "modConfig.json");
        }

        public static void EnableMods(Config config)
        {
            ConsoleUI.ShowInfo("Restoring mods...");

            foreach (var path in config.ModPaths)
            {
                var fileName = Path.GetFileName(path);
                var backupPath = Path.Combine(BackupFolder, fileName);

                if (File.Exists(backupPath))
                {
                    File.Move(backupPath, path, true);
                    ConsoleUI.ShowSuccess($"Restored: {backupPath} → {path}");
                }
                else if (Directory.Exists(backupPath))
                {
                    Directory.Move(backupPath, path);
                    ConsoleUI.ShowSuccess($"Folder restored: {backupPath} → {path}");
                }
                else
                {
                    ConsoleUI.ShowWarning($"Backup not found: {backupPath}");
                }
            }

            config.ModStatus = "Enabled";
            ConfigLoader.Save(config, "modConfig.json");
        }
    }

    static class ConsoleUI
    {
        public static void ShowBanner()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║     GTA V ModController by memcathzr       ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public static void ShowMenu()
        {
            Console.WriteLine("\n[E] Enable Mods");
            Console.WriteLine("[D] Disable Mods");
            Console.WriteLine("[Q] Quit");
            Console.Write("Your choice: ");
        }

        public static void ShowInfo(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[Info] " + message);
            Console.ResetColor();
        }

        public static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[✓] " + message);
            Console.ResetColor();
        }

        public static void ShowWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[Warning] " + message);
            Console.ResetColor();
        }

        public static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] " + message);
            Console.ResetColor();
        }

        public static void ShowExit()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Exiting... Have fun!");
            Console.ResetColor();
        }
    }
}
