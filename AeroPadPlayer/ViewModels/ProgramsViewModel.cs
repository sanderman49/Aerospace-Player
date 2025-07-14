using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using aeropad_player.Audio;
using aeropad_player.Directory;
using AeroPadPlayer.Models;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace AeroPadPlayer.ViewModels;

public partial class ProgramsViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    
    public ICommand GoBack { get;  }
    public ICommand Play { get; }
    public ICommand Save { get; }
    public ICommand OpenPopup { get; }
    public ICommand ClosePopup { get; }

    public Playback Player { get; set; }
    
    private Program? _selectedProgram;
    public Program? SelectedProgram { get => _selectedProgram; set => this.RaiseAndSetIfChanged(ref _selectedProgram, value); }
    
    public Program? CurrentProgram { get; set; }
    
    public Aeropad Aeropad { get; }
    
    public ObservableCollection<Program>? Programs { get; set; }
    
    public string? Name { get; set; }
    public string? Patch { get; set; }
    public string? Scale { get; set; }
    public string? Key { get; set; }
    
    public string[] Patches { get; set; }
    public string[] Scales { get; set; }
    public string[] Keys { get; set; }
    

    private bool _showPopup;
    public bool ShowPopup
    {
        get => _showPopup;
        set { this.RaiseAndSetIfChanged(ref _showPopup, value); }
    }

    public ProgramsViewModel(IScreen screen, Playback player)
    {
        HostScreen = screen;
        Player = player;
        Aeropad = new Aeropad();
        Programs = new ObservableCollection<Program>();

        Play = ReactiveCommand.Create(PlayProgram);
        Save = ReactiveCommand.Create(SaveProgram);
        GoBack = ReactiveCommand.Create(() => HostScreen.Router.NavigationStack.Remove(this));
        OpenPopup = ReactiveCommand.Create(() => ShowPopup = true);
        ClosePopup = ReactiveCommand.Create(() => ShowPopup = false);

        Patches = Aeropad.Patches;
        Scales = Aeropad.Scales;
        Keys = Aeropad.Keys;
    }

    public void PlayProgram()
    {
        if (SelectedProgram?.Id == Player.CurrentProgram?.Id)
        {
            //Task.Run(() => Player.StopPad());
            Player.CurrentProgram = null;
            SelectedProgram = null;
            return;
        }

        Player.CurrentProgram = SelectedProgram;
        
        //string patch = Player.CurrentProgram.Patch;
        //string scale = Player.CurrentProgram.Scale;
        //string key = Player.CurrentProgram.Key;
        
        
        //Task.Run(() => Player.PlayPad(patch, scale, key)); 
    }

    public void SaveProgram()
    {
        // null means the Combobox hasn't been touched by the user.
        Programs.Add(new Program(Patch ?? Patches[0], Scale ?? Scales[0], Key ?? Keys[0], Name));
    }
}