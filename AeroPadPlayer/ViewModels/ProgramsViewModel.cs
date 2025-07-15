using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
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
    
    public ICommand Play { get; }
    public ICommand Save { get; }
    public ICommand OpenPopup { get; }
    public ICommand ClosePopup { get; }

    
    private Program? _selectedProgram;
    public Program? SelectedProgram { get => _selectedProgram; set => this.RaiseAndSetIfChanged(ref _selectedProgram, value); }
    

    private readonly Aeropad _aeropad;
    private readonly Playback _player;
    
    public ObservableCollection<Program> Programs { get; set; }
    
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
        _player = player;
        _aeropad = new Aeropad();
        Programs = Config.GetPrograms();

        Play = ReactiveCommand.Create(PlayProgram);
        Save = ReactiveCommand.Create(SaveProgram);
        OpenPopup = ReactiveCommand.Create(() => ShowPopup = true);
        ClosePopup = ReactiveCommand.Create(() => ShowPopup = false);

        Patches = _aeropad.Patches;
        Scales = _aeropad.Scales;
        Keys = _aeropad.Keys;
    }

    private void PlayProgram()
    {
        if (SelectedProgram?.Id == _player.CurrentProgram?.Id)
        {
            _player.CurrentProgram = null;
            SelectedProgram = null;
        }
        else
        {
            _player.CurrentProgram = SelectedProgram;
        }
    }
    
    public void OnViewLoad()
    {
        if (_player.CurrentProgram?.Id != SelectedProgram?.Id)
        {
            SelectedProgram = null;
        }
    }

    public void SaveProgram()
    {
        if (Name == null)
        {
            return;
        }
        
        // null means the Combobox hasn't been touched by the user.
        Programs.Add(new Program(Patch ?? Patches[0], Scale ?? Scales[0], Key ?? Keys[0], Name));
        
        Config.SavePrograms(Programs);
    }
}