using System;
using AeroPadPlayer.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AeroPadPlayer.Views;

public partial class ShellView : ReactiveUserControl<ShellViewModel>
{
    public ShellView()
    {
        InitializeComponent();
    }
    
}