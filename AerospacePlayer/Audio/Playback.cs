using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AerospacePlayer.Directory;
using AerospacePlayer.Models;
using DynamicData;
using SoundFlow.Abstracts;
using SoundFlow.Abstracts.Devices;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Enums;
using SoundFlow.Providers;
using SoundFlow.Structs;

namespace AerospacePlayer.Audio;

public class Playback
{
    private MiniAudioEngine _engine;
    private List<CustomSoundPlayer> _players;

    private AudioPlaybackDevice _audioPlaybackDevice;
    private Mixer _mixer;

    private AudioFormat _audioFormat;

    private Program? _currentProgram;


    private int _fadeTime;

    public int FadeTime
    {
        get => _fadeTime;
        set
        {
            _fadeTime = value;
        }
    }


    private float _pan;
    public float Pan
    {
        get => _pan;
        set
        {
            _pan = value;

            _mixer.Pan = FadeGain(value);
        }
    }

    
    private float _volume;
    public float Volume
    {
        get => _volume;
        set
        {
            _volume = value;

            _mixer.Volume = FadeGain(value);
        }
    }

    public Program? CurrentProgram
    {
        get => _currentProgram;
        set
        {
            _currentProgram = value;

            if (value != null)
            {
                PlayPad(value.Patch, value.Scale, value.Key);
            }

            if (value == null)
            {
                StopPad();
            }
        }
    }

    public Playback()
    {
        // Format, Sample-rate, channels.
        _audioFormat = AudioFormat.Cd;
        
        // Init audio engine.
        _engine = new MiniAudioEngine();
        
        // Setup main playback device. 
        _audioPlaybackDevice = _engine.InitializePlaybackDevice(null, AudioFormat.Cd);
        _engine.UpdateDevicesInfo();
        _audioPlaybackDevice.Start();
        _mixer = _audioPlaybackDevice.MasterMixer;
        
        // Init players list.
        _players = new List<CustomSoundPlayer>();

        // Will be overridden by settings.
        FadeTime = 5;
        Pan = 0.5f;
        Volume = 1f;
    }
    
    public async Task PlayPad(string patch, string scale, string key)
    {
        Aeropad aeropad = new Aeropad();
        
        // Find the path for the pad to play.
        string padPath = aeropad.FindPad(patch, scale, key);
        
        // Init the player.
        var file = File.OpenRead(padPath);
        var player = new CustomSoundPlayer(_engine, AudioFormat.Cd, new StreamDataProvider(_engine, AudioFormat.Cd, file));
        
        // Do this for some reason.
        _mixer.AddComponent(player);

        // Set player values.
        player.Volume = 0f; // Start with no volume.
        player.Pan = Pan;
        player.IsLooping = true;
        player.Play();

        // Stop all existing players. 
        await StopPad();

        _players.Add(player); // Add to players list

        // Fade in.
        while (player.Volume < Volume)
        {
            if (player.Cancelled)
            {
                break;
            }

            player.LinearVolume += 0.001f;
                
            player.Volume = FadeGain(player.LinearVolume + 0.001f);
            await Task.Delay(FadeTime);
        }

    }

    // Fades out all pads.
    public async Task StopPad()
    {
        foreach (var player in _players)
        {
            if (!player.Cancelled)
            {
                // Cancel any events associated with the player.
                player.Cancelled = true;
            
                // Fade all active players out at the same time.
                Task.Run(async () =>
                {
                    while (player.Volume > 0.001f)
                    {
                        player.LinearVolume -= 0.001f;
                    
                        player.Volume = FadeGain(player.LinearVolume - 0.001f);
                        await Task.Delay(FadeTime); // FadeTime = Seconds.
                    }

                    player.Stop();
                
                    // Clear the player from the players list and from the master.
                    _mixer.RemoveComponent(player);
                    _players.Remove(player);
                });
            }
        }
    }

    float FadeGain(float t)
    {
        float gain = MathF.Pow(t, 2.0f); // tweak exponent as needed 

        if (gain > 1f)
        {
            gain = 1f;
        } else if (gain < 0f)
        {
            gain = 0f;
        }
        return gain;
    }

    public void UpdateCurrentProgram(string? patch = null, string? scale = null, string? key = null)
    {
        if (CurrentProgram != null)
        {
            Program newProgram = CurrentProgram;
                
            if (!String.IsNullOrEmpty(patch))
            {
                newProgram.Patch = patch;
            }
            if (!String.IsNullOrEmpty(scale))
            {
                newProgram.Scale = scale;
            }
            if (!String.IsNullOrEmpty(key))
            {
                newProgram.Key = key;
            }

            CurrentProgram = newProgram;
        }
    }

    public bool CurrentProgramIsUserDefined()
    {
        bool isUserDefined;
        
        if (CurrentProgram != null)
            isUserDefined = CurrentProgram.IsUserDefined;
        else
            isUserDefined = false;
        
        return isUserDefined;
    }
    
}