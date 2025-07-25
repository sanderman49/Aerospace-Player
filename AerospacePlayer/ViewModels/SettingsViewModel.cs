using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using AerospacePlayer.Audio;
using AerospacePlayer.Directory;
using AerospacePlayer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace AerospacePlayer.ViewModels;

public partial class SettingsViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    
    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => HostScreen.Router.NavigateBack;

    private Playback _player;

    private Settings _settings;


    private int _fadeTime;
    public int FadeTime
    {
        get => _fadeTime ;
        set
        {
            _player.FadeTime = value;
            SettingsUpdated();
            this.RaiseAndSetIfChanged(ref _fadeTime, value);
            this.RaisePropertyChanged(nameof(FadeTimeFormatted));
        }
    }
    public string FadeTimeFormatted { get => $"{_fadeTime}s"; }


    private float _pan;
    public float Pan
    {
        get => _pan ;
        set
        {
            _player.Pan = value;
            SettingsUpdated();
            this.RaiseAndSetIfChanged(ref _pan, value);
        }
    }
    
    
    private float _volume;
    public float Volume
    {
        get => _volume ;
        set
        {
            _player.Volume = value;
            SettingsUpdated();
            this.RaiseAndSetIfChanged(ref _volume, value);
        }
    }

    public SettingsViewModel(IScreen screen, Playback player)
    {
        HostScreen = screen;
        _settings = Config.GetSettings();
        _player = player;

        // Pull in initial values from settings.
        _volume = _settings.Volume;
        _fadeTime = _settings.FadeTime;
        _pan = _settings.Pan;

        // Populate the player with imported settings.
        _player.Volume = _volume;
        _player.FadeTime = _fadeTime;
        _player.Pan = _pan;
    }

    public void SettingsUpdated()
    {
        _settings.FadeTime = FadeTime;
        _settings.Volume = Volume;
        _settings.Pan = Pan;
        
        Config.SaveSettings(_settings);
    }
}