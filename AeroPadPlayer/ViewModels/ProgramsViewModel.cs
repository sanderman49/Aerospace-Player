using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using aeropad_player.Audio;
using AeroPadPlayer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace AeroPadPlayer.ViewModels;

public partial class ProgramsViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    
    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => HostScreen.Router.NavigateBack;

    public Playback Player { get; set; }
    
    private Program? _selectedProgram;
    public Program? SelectedProgram { get => _selectedProgram; set => this.RaiseAndSetIfChanged(ref _selectedProgram, value); }
    
    public Program? CurrentProgram { get; set; }
    
    public ObservableCollection<Program>? Programs { get; set; }
    
    public string Name { get; set; }
    public string Patch { get; set; }
    public string Scale { get; set; }
    public string Key { get; set; }
    
    public ProgramsViewModel(IScreen screen, Playback player)
    {
        HostScreen = screen;
        Player = player;
        Programs = new ObservableCollection<Program>();

        Player.StopPad();
    }

    public void PlayProgram()
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
    }

    public void SaveProgram()
    {
        Programs.Add(new Program(Patch, Scale, Key, Name));
    }
        
}