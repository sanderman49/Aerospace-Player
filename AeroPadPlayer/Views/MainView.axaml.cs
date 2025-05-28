using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using aeropad_player.Audio;
using aeropad_player.Directory;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace AeroPadPlayer.Views;

public partial class MainView : UserControl
{
    private Playback playback;
    private Aeropad aeropad;
    private Config config;
    private string activeKey;
    public MainView()
    {
        Config.GenerateConfigPath();
        
        playback = new Playback();
        aeropad = new Aeropad();
        InitializeComponent();
    }

    public async void Play(object sender, RoutedEventArgs e)
    {
        string patch = aeropad.Patches[patchBox.SelectedIndex];
        string scale = aeropad.Scales[scaleBox.SelectedIndex];
        //string key = aeropad.Keys[keyBox.SelectedIndex];

        var source = e.Source as Control;
        var button = sender as Button;

        CButton.Opacity = 1;
        CSButton.Opacity = 1;
        DButton.Opacity = 1;
        DSButton.Opacity = 1;
        DSButton.Opacity = 1;
        EButton.Opacity = 1;
        FButton.Opacity = 1;
        FSButton.Opacity = 1;
        GButton.Opacity = 1;
        GSButton.Opacity = 1;
        AButton.Opacity = 1;
        ASButton.Opacity = 1;
        BButton.Opacity = 1;

        source.Opacity = 1.2;
        
        string key = button.Name.Substring(0, 1);
        
        if (button.Name.Substring(1, 1) == "S")
        {
            key = key + "#";
        }

        if (activeKey == key)
        {
            Task.Run(() => playback.StopPad());
            activeKey = "";
            source.Opacity = 1;
            return;
        }

        activeKey = key;
        
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