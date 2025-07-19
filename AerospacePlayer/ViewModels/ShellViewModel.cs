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

        programsViewModel = new ProgramsViewModel(this, _player);
        mainViewModel = new MainViewModel(this, _player);
        
        Router.Navigate.Execute(mainViewModel).Subscribe();
        
        GoToPrograms = ReactiveCommand.Create(() =>
        {
            if (Router.GetCurrentViewModel() != programsViewModel)
             Router.Navigate.Execute(programsViewModel).Subscribe();
        });
        GoToMain = ReactiveCommand.Create(() =>
        {
            if (Router.GetCurrentViewModel() != mainViewModel)
                Router.Navigate.Execute(mainViewModel).Subscribe();
        });
    }
}