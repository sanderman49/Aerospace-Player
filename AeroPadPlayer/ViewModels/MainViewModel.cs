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
using DynamicData;
using ReactiveUI;

namespace AeroPadPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public ICommand GoToSettings { get; }
    public ICommand Play { get; }


    private readonly float _activeOpacity = 1.2f;
    private readonly float _inactiveOpacity = 1f;
    private readonly Aeropad _aeropad;

    private AvaloniaList<float> _opacities;
    public AvaloniaList<float> Opacities
    {
        get => _opacities;
        set { this.RaiseAndSetIfChanged(ref _opacities, value); }
    }
    
    private readonly Playback _player;
    
    public string[] Patches { get; set; }
    public string[] Scales { get; set; }

    public string? Patch { get; set; }
    public string? Scale { get; set; }

    public MainViewModel(IScreen screen, Playback player)
    {
        HostScreen = screen;
        
        Opacities = new AvaloniaList<float>();
        for (int i = 0; i < 12; i++)
        {
            Opacities.Add(_inactiveOpacity);
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
        for (int i = 0; i < Opacities.Count; i++)
        {
            Opacities[i] = 1f;
        }

        Program selectedProgram = new Program(Patch ?? Patches[0], Scale ?? Scales[0], key);

        if (_player.CurrentProgram?.Signature == selectedProgram.Signature)
        {
            _player.CurrentProgram = null;
        }
        else
        {
            _player.CurrentProgram = selectedProgram;
            Opacities[OpacityKeyToIndex(key)] = _activeOpacity;
        }
    }

    public void OnViewLoad()
    {
        if (_player.CurrentProgram?.Name != null)
        {
            for (int i = 0; i < Opacities.Count; i++)
            {
                Opacities[i] = 1f;
            }
        }
    }

    private int OpacityKeyToIndex(string key)
    {
        return _aeropad.Keys.IndexOf(key);
    }
}