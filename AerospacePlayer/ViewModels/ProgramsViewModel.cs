using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AerospacePlayer.Audio;
using AerospacePlayer.Directory;
using AerospacePlayer.Models;
using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace AerospacePlayer.ViewModels;

public partial class ProgramsViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    
    public ICommand Play { get; }
    public ICommand Save { get; }
    public ICommand OpenAddPopup { get; }
    public ICommand CloseAddPopup { get; }
    public ICommand OpenDeletePopup { get; }
    public ICommand GoToEditProgram { get; }

    private string? _errorText;
    public string? ErrorText { get => _errorText; set => this.RaiseAndSetIfChanged(ref _errorText, value); }

    
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
        
        GoToEditProgram = ReactiveCommand.Create<Program>((program) => HostScreen.Router.Navigate.Execute(new EditProgramViewModel(HostScreen, program, Programs)));
        
        OpenAddPopup = ReactiveCommand.Create(() => ShowPopup = true);
        CloseAddPopup = ReactiveCommand.Create(() => ShowPopup = false);
        
        OpenDeletePopup = ReactiveCommand.Create(() => ShowPopup = true);

        Patches = _aeropad.Patches;
        Scales = _aeropad.Scales;
        Keys = _aeropad.Keys;
    }

    private void PlayProgram()
    {
        if (!(SelectedProgram == null && !_player.CurrentProgram.IsUserDefined)) // Don't clear the program from the Live screen
            _player.CurrentProgram = SelectedProgram;
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

        if (String.IsNullOrEmpty(Name))
        {
            ErrorText = "Programs must have a name.";
            return;
        }

        ErrorText = null;
        
        // null means the Combobox hasn't been touched by the user.
        Programs.Add(new Program(Patch ?? Patches[0], Scale ?? Scales[0], Key ?? Keys[0], Name));

        ShowPopup = false;
        
        Config.SavePrograms(Programs);
    }
}