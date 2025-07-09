using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using aeropad_player.Audio;
using aeropad_player.Directory;
using AeroPadPlayer.Models;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.ReactiveUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace AeroPadPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase, IRoutableViewModel
{
    //[ObservableProperty] private string _greeting = "Welcome to Avalonia!";

    public IScreen HostScreen { get; }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public ICommand GoToSettings { get; }

    public ICommand SaveProgram { get; }
    public ICommand PlayProgram { get; }

    private float activeOpacity = 1.2f;
    private float inactiveOpacity = 1f;

    public AvaloniaList<float> _opacities;

    public AvaloniaList<float> Opacities
    {
        get => _opacities;
        set { this.RaiseAndSetIfChanged(ref _opacities, value); }
    }

public ObservableCollection<Program>? Programs { get; set; }

    private Program? _selectedProgram;
    public Program? SelectedProgram { get => _selectedProgram; set => this.RaiseAndSetIfChanged(ref _selectedProgram, value); }
    
    public Playback Player;
    private Aeropad aeropad;
    private Config config;

    public ComboBoxItem Patch { get; set; }
    public int PatchIndex { get; set; }
    
    public ComboBoxItem Scale { get; set; }
    public int ScaleIndex { get; set; }
    
    public Program? CurrentProgram { get; set; }

    public string ActiveKey = "";

    public string Name { get; set; }


    public MainViewModel(IScreen screen)
    {
        Opacities = new AvaloniaList<float>()
        {
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
            1f,
        };
        
        HostScreen = screen;
        
        Programs = new ObservableCollection<Program>();
        
        Player = new Playback();
        aeropad = new Aeropad();

        GoToSettings = ReactiveCommand.CreateFromObservable(() => 
            HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen))
        );


        SaveProgram = ReactiveCommand.Create(() =>
        {
            if (CurrentProgram != null)
            {
                CurrentProgram.Name = Name;
                Programs.Add(CurrentProgram);
            }
        });

        PlayProgram = ReactiveCommand.Create(() =>
        {
            if (SelectedProgram?.Id == CurrentProgram?.Id)
            {
                Task.Run(() => Player.StopPad());
                CurrentProgram = null;
                SelectedProgram = null;
                return;
            }

            CurrentProgram = SelectedProgram;


            string patch = CurrentProgram.Patch;
            string scale = CurrentProgram.Scale;
            string key = CurrentProgram.Key;
        
            Task.Run(() => Player.PlayPad(patch, scale, key)); 
        });
        
        Config.GenerateConfigPath();
    }

    public void Play(string key)
    {
        string patch = Patch?.Content?.ToString() ?? "";
        string scale = Scale?.Content?.ToString() ?? "";

        Program? previousProgram = CurrentProgram;
            
        CurrentProgram = new Program(patch, scale, key);

        // Stop playing if already active.
        if (previousProgram?.Id == CurrentProgram.Id)
        {
            Task.Run(() => Player.StopPad());
            Opacities[OpacityKeyToIndex(key)] = inactiveOpacity;
            CurrentProgram = null;
            return;
        }

        for (int i = 0; i < Opacities.Count; i++)
        {
            Opacities[i] = 1f;
        }
        
        Opacities[OpacityKeyToIndex(key)] = activeOpacity;
        
        // Play the current note.
        Task.Run(() => Player.PlayPad(patch, scale, key));
    }

    private int OpacityKeyToIndex(string key)
    {
        switch (key)
        {
            case "C":
                return 0;
            case "C#":
                return 1;
            case "D":
                return 2;
            case "D#":
                return 3;
            case "E":
                return 4;
            case "F":
                return 5;
            case "F#":
                return 6;
            case "G":
                return 7;
            case "G#":
                return 8;
            case "A":
                return 9;
            case "A#":
                return 10;
            case "B":
                return 11;
            default:
                return -1;
        }
    }

}