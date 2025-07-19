using System;
using AerospacePlayer.Models;
using AerospacePlayer.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AerospacePlayer.Views;

public partial class ProgramsView : ReactiveUserControl<ProgramsViewModel>
{
    public ProgramsView()
    {
        InitializeComponent();
    }
    
    public void OnViewLoad(object? sender, RoutedEventArgs e)
    {
        var _viewModel = (ProgramsViewModel)DataContext;
        
        _viewModel.OnViewLoad();
    }

    private void ErrorTextUpdated(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        TextBlock textBlock = sender as TextBlock;

        if (String.IsNullOrEmpty(textBlock.Text))
            textBlock.IsVisible = false;
        else
            textBlock.IsVisible = true;
    }
}