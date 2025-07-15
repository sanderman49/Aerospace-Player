using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AerospacePlayer.Audio;
using AerospacePlayer.Directory;
using AerospacePlayer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace AerospacePlayer.ViewModels;

public class ShellViewModel : ViewModelBase, IScreen
{
    public RoutingState Router { get; }
    
    public ICommand GoToPrograms { get; set; }
    public ICommand GoToMain { get; set; }
    
    private ProgramsViewModel programsViewModel;
    private MainViewModel mainViewModel;
    
    private Playback _player;

    public ShellViewModel()
    {
        Config.GenerateConfigPath();
        
        Router = new RoutingState();
        _player = new Playback();

        Router.Navigate.Execute(new MainViewModel(this, _player)).Subscribe();
        
        programsViewModel = new ProgramsViewModel(this, _player);
        mainViewModel = new MainViewModel(this, _player);
        
        GoToPrograms = ReactiveCommand.Create(() => Router.Navigate.Execute(programsViewModel).Subscribe());
        GoToMain = ReactiveCommand.Create(() => Router.Navigate.Execute(mainViewModel).Subscribe());
    }
}