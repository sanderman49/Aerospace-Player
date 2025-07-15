using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using AerospacePlayer.Directory;
using AerospacePlayer.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ReactiveUI;

namespace AerospacePlayer.Views;

public partial class EditProgramView : ReactiveUserControl<EditProgramViewModel>
{
    public EditProgramView()
    {
        InitializeComponent();
    }
}