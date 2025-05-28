using System;
using System.IO;
using System.Runtime.InteropServices;

namespace aeropad_player.Directory;

public class Config
{
    // Generate the config directory if it doesn't exist.
    public static void GenerateConfigPath()
    {
        string configPath = GetConfigPath();
        
        if (!System.IO.Directory.Exists(configPath))
        {
            System.IO.Directory.CreateDirectory(configPath);
        }
    }
    
    // Get the config directory for each OS platform.
    public static string GetConfigPath()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "aeropad-player");
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "aeropad-player");
        }
        if (OperatingSystem.IsAndroid())
        {
            return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        }

        return "";
    }
    
    // The 'sounds' directory within the config directory.
    public static string GetPadLocation()
    {
        string configPath = GetConfigPath();
        string padPath = Path.Join(configPath, "sounds", "Complete Collection");

        return padPath;
    }
    
    // The 'sounds' directory within the config directory.
    public static string GetSoundsLocation()
    {
        string configPath = GetConfigPath();
        string padPath = Path.Join(configPath, "sounds");

        return padPath;
    }
}