# ModController for GTA 5 by memcathzr

🔧 A terminal-based mod toggler for Grand Theft Auto V.  
It enables or disables selected mod files and folders using a simple console interface. Configured via `modConfig.json`, the tool safely moves mods to and from the `ModsBackup/` folder, keeping your game directory clean for mod-free launches.

📥 **Download v1.1:** [![Download](https://img.shields.io/badge/Download-v1.1-blue.svg)](https://github.com/memcathzr/ModController-for-GTA-5-by-memcathzr/releases/download/v1.1/ModController.for.GTA.5.by.memcathzr.v1.1.zip)



---

## 🧩 Installation

1. Download the latest release: [`ModController-v1.1.zip`]([https://github.com/memcathzr/modcontroller/releases/latest](https://github.com/memcathzr/ModController-for-GTA-5-by-memcathzr/releases/download/v1.1/ModController.for.GTA.5.by.memcathzr.v1.1.zip))
2. Extract all files next to your GTA V executable (`GTA5.exe`)
3. Create or edit a valid `modConfig.json` file
4. Run `ModController.exe` and choose your desired action

> 🔸 `ModsBackup/` will be created automatically if missing





---

## 📝 modConfig.json Example

```json
{
  "ModPaths": [
    "mods",
    "scripts//c",
    "dinput8.dll"
  ],
  "ModStatus": "Disabled"
}
```

What does it mean?
It puts the mods folder, the c folder in the scripts folder, and the dinput8.dll file in the ModsBackup folder.

The program also tracks mod status ("Enabled" or "Disabled") inside modConfig.json.

All paths are relative to the EXE location

🖥️ Terminal Menu,

E → Enable Mods     (restores files from ModsBackup)

D → Disable Mods    (moves files into ModsBackup)

Q → Quit            (quits the program)

Conflicts trigger overwrite confirmation

Missing items are skipped with warnings

🌟 Features
✅ Instant mod toggling via terminal

✅ Safe folder/file transfer system

✅ Interactive overwrite prompts

✅ Supports both folders and single files

✅ Configuration via simple JSON

✅ Open-source under GPL v3

🎮 Why Use ModController?

Whether you're switching between singleplayer modding and online play, ModController makes it easy to isolate and restore your mod files safely — all without renaming, dragging, or deleting files manually.

⚙️ Build Instructions

Requires .NET 8 SDK

dotnet publish -c Release -r win-x64 --self-contained false

This produces a standalone .exe without dependencies

🔓 License

This project is licensed under the GNU GPL v3.

You are free to use, modify, and distribute this tool, but any modified versions must also remain open-source under the same license.

🌐 Repository

Original source: https://github.com/memcathzr/ModController-for-GTA-5-by-memcathzr Pull requests and contributions are welcome!




