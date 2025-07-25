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
    public ICommand GoToPatchSelect { get; }
    public ICommand Play { get; }


    private SettingsViewModel _settingsViewModel;


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

    private string? _patch;
    public string? Patch
    {
        get => _patch;
        set
        {
            _patch = value;

            if (!_player.CurrentProgramIsUserDefined())
                _player.UpdateCurrentProgram(patch: value);
        }
    }

    private string? _scale;
    public string? Scale
    {
        get => _scale;
        set
        {
            _scale = value;

            if (!_player.CurrentProgramIsUserDefined())
                _player.UpdateCurrentProgram(scale: value);
        }
    }

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

        Patch = _aeropad.Patches[0];

        _settingsViewModel = new SettingsViewModel(HostScreen, _player);
        
        GoToSettings = new RelayCommand(() => HostScreen.Router.Navigate.Execute(_settingsViewModel));
        
        GoToPatchSelect = new RelayCommand(() => HostScreen.Router.Navigate.Execute(new PatchSelectViewModel(HostScreen, this)));
        
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
            IsActiveKeys[KeyToIndex(key)] = true;
        }
    }

    public void OnViewLoad()
    {
        if (_player.CurrentProgramIsUserDefined())
        {
            for (int i = 0; i < IsActiveKeys.Count; i++)
            {
                IsActiveKeys[i] = false;
            }
        }
    }

    private int KeyToIndex(string key)
    {
        return _aeropad.Keys.IndexOf(key);
    }
}