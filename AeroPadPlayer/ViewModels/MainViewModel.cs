using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using aeropad_player.Audio;
using aeropad_player.Directory;
using AeroPadPlayer.Models;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.ReactiveUI;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReactiveUI;

namespace AeroPadPlayer.ViewModels;

public partial class MainViewModel : ViewModelBase, IRoutableViewModel
{
    //[ObservableProperty] private string _greeting = "Welcome to Avalonia!";
    
    public IScreen HostScreen { get; }
    
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);
    
    public ICommand GoToSettings { get; }
    public ICommand Play { get; }
    
    public ICommand SaveProgram { get; }
    public ICommand PlayProgram { get; }
    
    public ObservableCollection<Program>? Programs { get; set; }

    private Program? _selectedProgram;
    public Program? SelectedProgram { get => _selectedProgram; set => this.RaiseAndSetIfChanged(ref _selectedProgram, value); }
    
    public Playback Player;
    private Aeropad aeropad;
    private Config config;

    public ComboBoxItem Patch { get; set; }
    public int PatchIndex { get; set; }
    
    public ComboBoxItem Scale { get; set; }
    public int ScaleIndex { get; set; }
    
    public Program? CurrentProgram { get; set; }

    public string ActiveKey = "";

    public string Name { get; set; }


    public MainViewModel(IScreen screen)
    {
        HostScreen = screen;
        
        Programs = new ObservableCollection<Program>();
        
        Player = new Playback();
        aeropad = new Aeropad();

        GoToSettings = ReactiveCommand.CreateFromObservable(() => 
            HostScreen.Router.Navigate.Execute(new SettingsViewModel(HostScreen))
        );

        Play = ReactiveCommand.Create<string>((key) =>
        {
            string patch = Patch?.Content?.ToString() ?? "";
            string scale = Scale?.Content?.ToString() ?? "";

            Program? previousProgram = CurrentProgram;
            
            CurrentProgram = new Program(patch, scale, key);

            // Stop playing if already active.
            if (previousProgram?.Id == CurrentProgram.Id)
            { 
                Task.Run(() => Player.StopPad());
                //ActiveKey = "";
                //source.Opacity = 1;
                CurrentProgram = null;
                return;
            }
                
            //ActiveKey = key;
            

            //if (ProgramList.SelectedItem != null)
            //{
            //    ProgramList.SelectedItem = null;
            //}
            
            // Play the current note.
            Task.Run(() => Player.PlayPad(patch, scale, key));

        });

        SaveProgram = ReactiveCommand.Create(() =>
        {
            if (CurrentProgram != null)
            {
                CurrentProgram.Name = Name;
                Programs.Add(CurrentProgram);
            }
            
        });

        PlayProgram = ReactiveCommand.Create(() =>
        {
            if (SelectedProgram?.Id == CurrentProgram?.Id)
            {
                Task.Run(() => Player.StopPad());
                CurrentProgram = null;
                SelectedProgram = null;
                return;
            }

            CurrentProgram = SelectedProgram;


            string patch = CurrentProgram.Patch;
            string scale = CurrentProgram.Scale;
            string key = CurrentProgram.Key;
        
            Task.Run(() => Player.PlayPad(patch, scale, key)); 
        });
        
        Config.GenerateConfigPath();
    }

        
}