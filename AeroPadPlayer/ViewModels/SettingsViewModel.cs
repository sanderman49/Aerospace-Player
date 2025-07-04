using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using AeroPadPlayer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;

namespace AeroPadPlayer.ViewModels;

public partial class SettingsViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    
    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => HostScreen.Router.NavigateBack;
    
    public SettingsViewModel(IScreen screen)
    {
        HostScreen = screen;

    }
        
}