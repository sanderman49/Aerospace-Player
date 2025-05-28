using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using aeropad_player.Directory;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Components;
using SoundFlow.Enums;
using SoundFlow.Providers;

namespace aeropad_player.Audio;

public class Playback
{
    private MiniAudioEngine audioEngine;
    private List<CustomSoundPlayer> players;
    
    public Playback()
    {
        // Create the actual audio session.
        audioEngine = new MiniAudioEngine(44100, Capability.Playback, SampleFormat.S16);
        
        // Init players list.
        players = new List<CustomSoundPlayer>();
    }
    
    public async Task PlayPad(string patch, string scale, string key)
    {
        Aeropad aeropad = new Aeropad();
        
        // Find the path for the pad to play.
        string padPath = aeropad.FindPad(patch, scale, key);
        
        // Init the player.
        var file = File.OpenRead(padPath);
        var player = new CustomSoundPlayer(new ChunkedDataProvider(file));
        
        // Do this for some reason.
        Mixer.Master.AddComponent(player);

        // Set player values.
        player.Volume = 0f; // Start with no volume.
        player.Pan = 0.5f; // Stops audio from only going out of one ear when changing vol.
        player.IsLooping = true; // Loop infinitely.
        player.Play();

        // Stop all existing players. 
        await StopPad();

        players.Add(player);

        while (player.Volume < 1f)
        {
            if (player.Cancelled)
            {
                break;
            }
            player.Volume = player.Volume + 0.01f;
            await Task.Delay(50);
        }
    }

    // Fades out all pads.
    public async Task StopPad()
    {
        foreach (var player in players)
        {
            // Cancel any events associated with the player.
            player.Cancelled = true;
            
            // Fade all active players out at the same time.
            Task.Run(async () =>
            {
                // 5 seconds with 0.001f decrements and 5ms delay.
                while (player.Volume > 0.01f)
                {
                    player.Volume = player.Volume - 0.01f;
                    await Task.Delay(50);
                }

                // Stop the player.
                player.Stop();
                
                // Clear the player from the players list and from the master.
                Mixer.Master.RemoveComponent(player);
                players.Remove(player);
            });
        }
    }
    
    // Fades out a single pad.
    public async Task StopPad(CustomSoundPlayer player)
    {
        // 5 seconds with 0.001f decrements and 5ms delay.
        while (player.Volume > 0.01f)
        {
            player.Volume = player.Volume - 0.01f;
            await Task.Delay(50);
        }
        player.Stop(); 
    }
    
}