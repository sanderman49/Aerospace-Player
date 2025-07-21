using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Windows.Input;
using AerospacePlayer.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace AerospacePlayer.ViewModels;

public partial class PatchSelectViewModel : ViewModelBase, IRoutableViewModel
{
    public IScreen HostScreen { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    
    public string[] Patches { get; set; }
    
    public ReactiveCommand<Unit, IRoutableViewModel> GoBack => HostScreen.Router.NavigateBack;
    
    public ICommand SelectAndReturn { get; }
    
    public string Patch { get; set; }
    
    public PatchSelectViewModel(IScreen screen, MainViewModel mainViewModel)
    {
        HostScreen = screen;

        Patches = mainViewModel.Patches;
        
        SelectAndReturn = ReactiveCommand.Create<string>((selectedPatch) =>
        {
            mainViewModel.Patch = selectedPatch;
            
            HostScreen.Router.NavigationStack.Remove(this);
        });

    }
        
}