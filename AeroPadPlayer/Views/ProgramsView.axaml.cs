using AeroPadPlayer.Models;
using AeroPadPlayer.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace AeroPadPlayer.Views;

public partial class ProgramsView : UserControl, IViewFor<ProgramsViewModel>
{
    public ProgramsView()
    {
        InitializeComponent();
    }
    
    // These two properties are required by IViewFor<TViewModel>
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (ProgramsViewModel?)value;
    }

    public ProgramsViewModel? ViewModel { get; set; } 
}