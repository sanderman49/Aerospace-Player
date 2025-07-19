using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using AerospacePlayer.Audio;
using AerospacePlayer.Directory;
using AerospacePlayer.Models;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.ReactiveUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;

namespace AerospacePlayer.ViewModels;

public partial class MainViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public ICommand GoToSettings { get; }
    public ICommand Play { get; }


    private readonly Aeropad _aeropad;

    private AvaloniaList<bool> _isActiveKeys;
    public AvaloniaList<bool> IsActiveKeys
    {
        get => _isActiveKeys;
        set { this.RaiseAndSetIfChanged(ref _isActiveKeys, value); }
    }
    
    private readonly Playback _player;
    
    public string[] Patches { get; set; }
    public string[] Scales { get; set; }

    public string? Patch { get; set; }
    public string? Scale { get; set; }

    public MainViewModel(IScreen screen, Playback player)
    {
        HostScreen = screen;
        
        IsActiveKeys = new AvaloniaList<bool>();
        
        for (int i = 0; i < 12; i++)
        {
            IsActiveKeys.Add(false);
        }
        
        _player = player;
        _aeropad = new Aeropad();
        
        Patches = _aeropad.Patches;
        Scales = _aeropad.Scales;

        GoToSettings = new RelayCommand(() =>
            {
                HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen));
            }
        );
        
        Play = ReactiveCommand.Create<string>(PlayPad);
    }

    private void PlayPad(string key)
    {
        for (int i = 0; i < IsActiveKeys.Count; i++)
        {
            IsActiveKeys[i] = false;
        }

        Program selectedProgram = new Program(Patch ?? Patches[0], Scale ?? Scales[0], key);

        if (_player.CurrentProgram?.Signature == selectedProgram.Signature)
        {
            _player.CurrentProgram = null;
        }
        else
        {
            _player.CurrentProgram = selectedProgram;
            IsActiveKeys[OpacityKeyToIndex(key)] = true;
        }
    }

    public void OnViewLoad()
    {
        if (_player.CurrentProgram?.Name != null)
        {
            for (int i = 0; i < IsActiveKeys.Count; i++)
            {
                IsActiveKeys[i] = false;
            }
        }
    }

    private int OpacityKeyToIndex(string key)
    {
        return _aeropad.Keys.IndexOf(key);
    }
}