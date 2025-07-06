using System;
using System.IO;
using System.Text.Json;

class ConfigModel
{
    public string[] ModPaths { get; set; }
}

class Program
{
    static string exeDir = AppDomain.CurrentDomain.BaseDirectory;
    static string configPath = Path.Combine(exeDir, "modConfig.json");
    static string backupDir = Path.Combine(exeDir, "ModsBackup");

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "ModController Terminal";

        if (!File.Exists(configPath))
        {
            Console.WriteLine("❌ 'modConfig.json' not found beside the executable.");
            Console.ReadKey();
            return;
        }

        if (!Directory.Exists(backupDir))
        {
            Directory.CreateDirectory(backupDir);
            Console.WriteLine("📁 'ModsBackup' folder created.");
        }

        while (true)
        {
            ShowHeader();
            Console.WriteLine("📢 Available Actions:");
            Console.WriteLine("  1 → Enable Mods");
            Console.WriteLine("  2 → Disable Mods");
            Console.WriteLine("  3 → How to Use");
            Console.WriteLine("  0 → Exit");
            Console.Write("\n🔍 Enter your choice: ");
            string input = Console.ReadLine()?.Trim().ToLower();

            switch (input)
            {
                case "1": EnableMods(); break;
                case "2": DisableMods(); break;
                case "3": ShowHelp(); break;
                case "0": return;
                default: Console.WriteLine("⚠️ Unknown command. Try 1, 2, 3 or 0."); break;
            }

            Console.WriteLine("\n🔄 Press any key to return to the menu...");
            Console.ReadKey();
        }
    }

    static void EnableMods()
    {
        var config = JsonSerializer.Deserialize<ConfigModel>(File.ReadAllText(configPath));

        foreach (string path in config.ModPaths)
        {
            string source = Path.Combine(backupDir, path);
            string target = Path.Combine(exeDir, path);

            try
            {
                if (Directory.Exists(source))
                {
                    if (Directory.Exists(target) && !ConfirmOverwrite(target))
                    {
                        Console.WriteLine($"⏩ Skipped folder: {path}");
                        continue;
                    }

                    CopyDirectory(source, target);
                    Directory.Delete(source, true);
                    Console.WriteLine($"📂 Folder restored: {path}");
                }
                else if (File.Exists(source))
                {
                    if (File.Exists(target) && !ConfirmOverwrite(target))
                    {
                        Console.WriteLine($"⏩ Skipped file: {path}");
                        continue;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                    File.Copy(source, target, true);
                    File.Delete(source);
                    Console.WriteLine($"📄 File restored: {path}");
                }
                else
                {
                    Console.WriteLine($"⚠️ Missing in backup: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error restoring '{path}': {ex.Message}");
            }
        }
    }

    static void DisableMods()
    {
        var config = JsonSerializer.Deserialize<ConfigModel>(File.ReadAllText(configPath));

        foreach (string path in config.ModPaths)
        {
            string source = Path.Combine(exeDir, path);
            string target = Path.Combine(backupDir, path);

            try
            {
                if (Directory.Exists(source))
                {
                    if (Directory.Exists(target) && !ConfirmOverwrite(target))
                    {
                        Console.WriteLine($"⏩ Skipped folder: {path}");
                        continue;
                    }

                    CopyDirectory(source, target);
                    Directory.Delete(source, true);
                    Console.WriteLine($"📁 Folder backed up: {path}");
                }
                else if (File.Exists(source))
                {
                    if (File.Exists(target) && !ConfirmOverwrite(target))
                    {
                        Console.WriteLine($"⏩ Skipped file: {path}");
                        continue;
                    }

                    Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                    File.Copy(source, target, true);
                    File.Delete(source);
                    Console.WriteLine($"📄 File backed up: {path}");
                }
                else
                {
                    Console.WriteLine($"⚠️ Not found: {path}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error backing up '{path}': {ex.Message}");
            }
        }
    }

    static void CopyDirectory(string sourceDir, string targetDir)
    {
        Directory.CreateDirectory(targetDir);

        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string targetFile = Path.Combine(targetDir, Path.GetFileName(file));
            File.Copy(file, targetFile, true);
        }

        foreach (string folder in Directory.GetDirectories(sourceDir))
        {
            string subTarget = Path.Combine(targetDir, Path.GetFileName(folder));
            CopyDirectory(folder, subTarget);
        }
    }

    static bool ConfirmOverwrite(string targetPath)
    {
        Console.WriteLine($"\n⚠️ '{targetPath}' already exists.");
        Console.Write("Do you want to overwrite it? (y/n): ");
        string input = Console.ReadLine()?.Trim().ToLower();
        return input == "y" || input == "yes";
    }

    static void ShowHeader()
    {
        Console.Clear();
        Console.WriteLine("════════════════════════════════════════════════");
        Console.WriteLine("🔧 ModController for GTA 5 by memcathzr Version:1.0");
        Console.WriteLine("🌐 GitHub: https://github.com/memcathzr/ModController-for-GTA-5-by-memcathzr");
        Console.WriteLine("🛠️ Open Source | Licensed under GNU GPL v3");
        Console.WriteLine("════════════════════════════════════════════════\n");
    }

    static void ShowHelp()
    {
        Console.Clear();
        Console.WriteLine("════════════════════════════════════════════════════");
        Console.WriteLine("📘 HOW TO USE MODCONTROLLER");
        Console.WriteLine("════════════════════════════════════════════════════");
        Console.WriteLine("📄 Required Files:");
        Console.WriteLine("• modConfig.json — must be beside the executable");
        Console.WriteLine("• ModsBackup      — will be auto-created if missing\n");

        Console.WriteLine("🔧 Commands:");
        Console.WriteLine("1 → Enable Mods     — restores files from ModsBackup");
        Console.WriteLine("2 → Disable Mods    — moves mods to ModsBackup");
        Console.WriteLine("3 → How to Use      — shows this help screen");
        Console.WriteLine("0 → Exit            — closes the program\n");

        Console.WriteLine("📝 modConfig.json Example:");
        Console.WriteLine("{");
        Console.WriteLine("  \"ModPaths\": [");
        Console.WriteLine("    \"a\",");
        Console.WriteLine("    \"b/scripts\",");
        Console.WriteLine("    \"menyoo.dll\"");
        Console.WriteLine("  ]");
        Console.WriteLine("}\n");

        Console.WriteLine("💡 Notes:");
        Console.WriteLine("• Paths must be relative to the EXE");
        Console.WriteLine("• Supports folders and individual files");
        Console.WriteLine("• Prompts before overwriting existing files");
        Console.WriteLine("• This program should be in the GTA 5 folder");
        Console.WriteLine("════════════════════════════════════════════════════");
        Console.WriteLine("\n🔁 Press any key to return to menu...");
        Console.ReadKey();
    }
}
