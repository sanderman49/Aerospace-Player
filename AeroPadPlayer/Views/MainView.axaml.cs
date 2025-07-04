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

public partial class MainView : UserControl, IViewFor<MainViewModel>
{
    
    private MainViewModel _viewModel;
    
    private List<Button> buttons;
    
    public string activeKey;

    private Aeropad aeropad;
    private Playback playback;
    
    private Program currentProgram;

    public MainView()
    {
        
        InitializeComponent();
        
        buttons = new List<Button>()
        {
            CButton,
            CSButton,
            DButton,
            DSButton,
            EButton,
            FButton,
            FSButton,
            GButton,
            GSButton,
            AButton,
            ASButton,
            BButton,
        };

        patchBox.SelectedValue = ViewModel?.Patch;
        scaleBox.SelectedValue = ViewModel?.Scale;
    }
    
    // These two properties are required by IViewFor<TViewModel>
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (MainViewModel?)value;
    }

    public MainViewModel? ViewModel { get; set; } 

    public async void Play(object sender, RoutedEventArgs e)
    {
        string patch = aeropad.Patches[patchBox.SelectedIndex];
        string scale = aeropad.Scales[scaleBox.SelectedIndex];
        //string key = aeropad.Keys[keyBox.SelectedIndex];

        var source = e.Source as Control;
        var button = sender as Button;

        // Set all the buttons to normal opacity (this is terrible).
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].Opacity = 1;
        }

        // Makes the button darker for some reason.
        source.Opacity = 1.2;
        
        string key = button.Name.Substring(0, 1);
        
        // Replace the 'S' for sharp with the actual hash symbol.
        if (button.Name.Substring(1, 1) == "S")
        {
            key = key + "#";
        }

        // Stop playing if already active.
        if (activeKey == key)
        {
            await Task.Run(() => playback.StopPad());
            activeKey = "";
            source.Opacity = 1;
            return;
        }
        activeKey = key;
        
        currentProgram = new Program(patch, scale, key);

        if (ProgramList.SelectedItem != null)
        {
            ProgramList.SelectedItem = null;
        }
        
        // Play the current note.
        Task.Run(() => playback.PlayPad(patch, scale, key));
    }

    public async void fileWizard(object sender, RoutedEventArgs e)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = TopLevel.GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open Text File",
            AllowMultiple = false,
            FileTypeFilter = new[]
            { 
                new FilePickerFileType("Zip Files") { Patterns = new[] { "*.zip" } },
                new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
            }
        });

        if (files.Count >= 1)
        {
            await ExtractZip(files);
        }

    }

    public async void saveProgram(object sender, RoutedEventArgs e)
    {
        _viewModel = (MainViewModel)DataContext;
        
        _viewModel.CurrentProgram.Name = Name.Text;
        
        _viewModel.Programs.Add(currentProgram);
    }

    public async void playProgram(object sender, RoutedEventArgs e)
    {
        var listBox = sender as ListBox;

        if (listBox.SelectedItem == null)
        {
            return;
        }
        
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].Opacity = 1;
        }
        
        Program selectedProgram = listBox.SelectedItem as Program;

        string patch = selectedProgram.Patch;
        string scale = selectedProgram.Scale;
        string key = selectedProgram.Key;
        
        // Play the current note.
        Task.Run(() => playback.PlayPad(patch, scale, key));
    }

    public async void GoToSettings(object sender, RoutedEventArgs e)
    {
    }
    
    private async Task ExtractZip(IReadOnlyList<IStorageFile> files)
    {
        string extractPath = Config.GetSoundsLocation();
        
        await using var stream = await files[0].OpenReadAsync();
        using (var archive = new ZipArchive(stream, ZipArchiveMode.Read))
        {
            int extracted = 0;
            int totalFiles = archive.Entries.Count;
                
            foreach (var entry in archive.Entries)
            {
                string destinationPath = Path.Combine(extractPath, entry.FullName);
                if (destinationPath.Contains("Tropsophere"))
                {
                    destinationPath = destinationPath.Replace("Tropsophere", "Troposphere");
                }
                if (destinationPath.Contains("Expsphere"))
                {
                    destinationPath = destinationPath.Replace("Expsphere", "Exosphere");
                }
                
                string destinationDir = Path.GetDirectoryName(destinationPath);

                // Create directory if it doesn't exist
                if (!string.IsNullOrEmpty(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir);
                }
                
                // Skip if it's a directory entry
                if (string.IsNullOrEmpty(entry.Name))
                    continue; 
                
                using (var entryStream = entry.Open())
                using (var fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write))
                {
                    entryStream.CopyTo(fileStream);
                }
                extracted++;
                Console.WriteLine($"Extracted {entry.FullName}. {extracted} of {totalFiles} files.");
            }
            Console.WriteLine("Done");
            
        }
    }
}