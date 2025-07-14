using System;
using System.Collections.ObjectModel;
using AeroPadPlayer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace AeroPadPlayer.ViewModels;

public class ShellViewModel : ViewModelBase, IScreen
{
    public RoutingState Router { get; }

    public ShellViewModel()
    {
        Router = new RoutingState();

        Router.Navigate.Execute(new MainViewModel(this)).Subscribe();
    }
}