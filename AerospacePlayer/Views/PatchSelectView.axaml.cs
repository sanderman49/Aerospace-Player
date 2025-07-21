using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using AerospacePlayer.Directory;
using AerospacePlayer.ViewModels;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;

namespace AerospacePlayer.Views;

public partial class PatchSelectView : ReactiveUserControl<PatchSelectViewModel>
{
    public PatchSelectView()
    {
        InitializeComponent();
    }
    
}