using System;
using System.Collections.ObjectModel;
using System.Reactive;
using AeroPadPlayer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace AeroPadPlayer.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IScreen
{
    public RoutingState Router { get; }
    
    public MainWindowViewModel()
    {
        Router = new RoutingState();
        
        Router.Navigate.Execute(new MainViewModel(this)).Subscribe();
    }
}