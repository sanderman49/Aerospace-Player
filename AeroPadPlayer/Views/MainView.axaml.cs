using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using aeropad_player.Audio;
using aeropad_player.Directory;
using AeroPadPlayer.Models;
using AeroPadPlayer.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AeroPadPlayer.Views;

public partial class MainView : ReactiveUserControl<MainViewModel>
{
    public MainView()
    {
        InitializeComponent();
    }

    public void OnViewLoad(object? sender, RoutedEventArgs e)
    {
        var _viewModel = (MainViewModel)DataContext;
        
        _viewModel.OnViewLoad();
    }
}