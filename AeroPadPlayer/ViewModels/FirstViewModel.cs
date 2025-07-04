using System;
using System.Collections.ObjectModel;
using AeroPadPlayer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace AeroPadPlayer.ViewModels;

public class FirstViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }

    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public FirstViewModel(IScreen screen) => HostScreen = screen;
}