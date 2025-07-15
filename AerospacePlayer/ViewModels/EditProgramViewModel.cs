using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using AerospacePlayer.Directory;
using AerospacePlayer.Models;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace AerospacePlayer.ViewModels;

public partial class EditProgramViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    
    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => HostScreen.Router.NavigateBack;
    
    public ICommand Save { get; }
    public ICommand Delete { get; }

    private Program _program;
    private ObservableCollection<Program> _programs;
    
    private readonly Aeropad _aeropad;
    
    public string[] Patches { get; set; }
    public string[] Scales { get; set; }
    public string[] Keys { get; set; }
    
    public string Patch { get; set; }
    public string Scale { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    
    
    public EditProgramViewModel(IScreen screen, Program program, ObservableCollection<Program> programs)
    {
        HostScreen = screen;
        _program = program;
        _programs = programs;
        _aeropad = new Aeropad();
        
        Patches = _aeropad.Patches;
        Scales = _aeropad.Scales;
        Keys = _aeropad.Keys;

        Name = _program.Name;
        Patch = _program.Patch;
        Scale = _program.Scale;
        Key = _program.Key;
        
        Save = ReactiveCommand.Create(() =>
        {
            _program.Name = Name;
            _program.Patch = Patch ?? _program.Patch;
            _program.Scale = Scale ?? _program.Scale;
            _program.Key = Key ?? _program.Key;
            
            HostScreen.Router.NavigationStack.Remove(this);
        });
        
        Delete = ReactiveCommand.Create(() =>
        {
            _programs.Remove(_program);
            
            HostScreen.Router.NavigationStack.Remove(this);
        });

    }

}