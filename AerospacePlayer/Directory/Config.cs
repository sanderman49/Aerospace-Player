using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using AerospacePlayer.Models;

namespace AerospacePlayer.Directory;

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
            return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "aerospace-player");
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library",
                "Application Support", "aerospace-player");
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "aerospace-player");
        }
        if (OperatingSystem.IsAndroid())
        {
            return Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        }

        return "";
    }

    public static void SavePrograms(ObservableCollection<Program> programs)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };
        
        var serializedPrograms = JsonSerializer.Serialize(programs, options);

        string programsPath = GetProgramsPath();
        
        File.WriteAllText(programsPath, serializedPrograms);
    }
    
    
    public static ObservableCollection<Program>? GetPrograms()
    {
        string programsPath = GetProgramsPath();

        string serializedPrograms;

        try
        {
            serializedPrograms = File.ReadAllText(programsPath);
        }
        catch (FileNotFoundException)
        {
            return new ObservableCollection<Program>();
        }

        var programs = JsonSerializer.Deserialize<ObservableCollection<Program>>(serializedPrograms);

        return programs;
    }
    
    public static void SaveSettings(Settings settings)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };
        
        var serializedSettings = JsonSerializer.Serialize(settings, options);

        string settingsPath = GetSettingsPath();
        
        File.WriteAllText(settingsPath, serializedSettings);
    }
    
    public static Settings GetSettings()
    {
        string settingsPath = GetSettingsPath();

        string serializedSettings;

        try
        {
            serializedSettings = File.ReadAllText(settingsPath);
        }
        catch (FileNotFoundException)
        {
            return new Settings();
        }

        var settings = JsonSerializer.Deserialize<Settings>(serializedSettings);

        return settings;
    }

    public static string GetProgramsPath()
    {
        var configPath = GetConfigPath();
        var programsPath = Path.Join(configPath, "programs.json");

        return programsPath;
    }
    
    public static string GetSettingsPath()
    {
        var configPath = GetConfigPath();
        var programsPath = Path.Join(configPath, "settings.json");

        return programsPath;
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