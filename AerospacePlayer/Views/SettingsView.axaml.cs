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

public partial class SettingsView : ReactiveUserControl<SettingsViewModel>
{
    public SettingsView()
    {
        InitializeComponent();
    }
    
    public async void FileWizard(object sender, RoutedEventArgs e)
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
                    System.IO.Directory.CreateDirectory(destinationDir);
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