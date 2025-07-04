using CommunityToolkit.Mvvm.ComponentModel;

namespace AeroPadPlayer.Models;

public class Program : ObservableObject
{
    public string? Id { get => $"{Name}{Patch}{Scale}{Key}"; }
    public string? Name { get; set; }
    
    public string Patch { get; set; }
    public string Scale { get; set; }
    public string Key { get; set; }

    public Program(string patch, string scale, string key, string name)
    {
        Patch = patch;
        Scale = scale;
        Key = key;
        Name = name;
    }
    
    public Program(string patch, string scale, string key)
    {
        Patch = patch;
        Scale = scale;
        Key = key;
    }
}