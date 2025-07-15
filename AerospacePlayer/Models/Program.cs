using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AerospacePlayer.Models;

public class Program : ObservableObject
{
    public string Signature { get => $"{Name}{Patch}{Scale}{Key}"; }
    public string Id { get; }
    
    public string? Name { get; set; }
    
    public string Patch { get; set; } = null!;
    public string Scale { get; set; } = null!;
    public string Key { get; set; } = null!;

    public Program(string patch, string scale, string key, string name)
    {
        Patch = patch;
        Scale = scale;
        Key = key;
        Name = name;
        
        Id = Guid.NewGuid().ToString();
    }
    
    public Program(string patch, string scale, string key)
    {
        Patch = patch;
        Scale = scale;
        Key = key;

        Id = Guid.NewGuid().ToString();
    }

    public Program()
    {
        Id = Guid.NewGuid().ToString();
    }
}