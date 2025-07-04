using AeroPadPlayer.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;

namespace AeroPadPlayer.Views;

public partial class SettingsView : UserControl, IViewFor<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();
    }
    
    // These two properties are required by IViewFor<TViewModel>
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (SettingsViewModel?)value;
    }

    public SettingsViewModel? ViewModel { get; set; } 
}