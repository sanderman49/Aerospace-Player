using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using AerospacePlayer.Audio;
using AerospacePlayer.Directory;
using AerospacePlayer.Models;
using AerospacePlayer.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace AerospacePlayer.ViewModels;

public class ShellViewModel : ViewModelBase, IScreen
{
    public RoutingState Router { get; }

    private bool _tabsVisible;
    public bool TabsVisible
    {
        get => _tabsVisible;
        set
        {
            this.RaiseAndSetIfChanged(ref _tabsVisible, value);
        }
    }

    private ProgramsViewModel programsViewModel;
    private MainViewModel mainViewModel;
    
    private Playback _player;

    public ShellViewModel()
    {
        Config.GenerateConfigPath();

        TabsVisible = true;
        
        Router = new RoutingState();
        _player = new Playback();

        programsViewModel = new ProgramsViewModel(this, _player);
        mainViewModel = new MainViewModel(this, _player);
        
        Router.Navigate.Execute(mainViewModel).Subscribe(vm  =>
        {
        });

        Router.NavigationChanged.Subscribe(_ =>
        {
            int vmNum = Router.NavigationStack.Count;
            
            // Hide navigation tabs when not on a main screen.
            if (vmNum <= 1)
            {
                TabsVisible = true;
            }
            else
            {
                TabsVisible = false;
            }
        });
    }

    public void NavigateToViewModel(string viewModel)
    {
        if (viewModel == nameof(MainViewModel))
        {
            Router.NavigateAndReset.Execute(mainViewModel).Subscribe();
        }
        
        if (viewModel == nameof(ProgramsViewModel))
        {
            Router.NavigateAndReset.Execute(programsViewModel).Subscribe();
        }
    }
}