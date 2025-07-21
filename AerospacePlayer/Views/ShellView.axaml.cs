using System;
using AerospacePlayer.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AerospacePlayer.Views;

public partial class ShellView : ReactiveUserControl<ShellViewModel>
{
    private ShellViewModel _viewModel;
    public ShellView()
    {
        
        InitializeComponent();
    }

    private void OnTabChanged(object? sender, SelectionChangedEventArgs e)
    {
        TabStrip tabStrip = sender as TabStrip;

        TabItem tabItem = tabStrip.SelectedValue as TabItem;
        
        if (_viewModel == null)
        {
            return;
        }

        if (tabItem.Name == "MainTab")
        {
            _viewModel.NavigateToViewModel(nameof(MainViewModel));
        }
        else if (tabItem.Name == "ProgramsTab")
        {
            _viewModel.NavigateToViewModel(nameof(ProgramsViewModel));
        }
    }

    private void OnDataContextChanged(object sender, EventArgs e)
    {
        _viewModel = (ShellViewModel)DataContext;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (OperatingSystem.IsAndroid() || OperatingSystem.IsIOS())
        {
            var insetsManager = TopLevel.GetTopLevel(this).InsetsManager;
            insetsManager.IsSystemBarVisible = true;
            insetsManager.SystemBarColor = Color.Parse("#1a1b26");
            
        }
    }
}