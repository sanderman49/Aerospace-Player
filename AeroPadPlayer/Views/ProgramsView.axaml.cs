using System;
using AeroPadPlayer.Models;
using AeroPadPlayer.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AeroPadPlayer.Views;

public partial class ProgramsView : ReactiveUserControl<ProgramsViewModel>
{
    public ProgramsView()
    {
        InitializeComponent();
    }
    
}