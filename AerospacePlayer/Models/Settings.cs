using AerospacePlayer.Audio;

namespace AerospacePlayer.Models;

public class Settings
{
    public int FadeTime { get; set; }
    
    public float Pan { get; set; }
    
    public float Volume { get; set; }
    
    public Settings()
    {
        FadeTime = 5;
        Pan = 0.5f;
        Volume = 1f;
    }
    
    public Settings(int fadeTime, float pan, float volume)
    {
        FadeTime = fadeTime;
        Pan = pan;
        Volume = volume;
    }
    
    public Settings(Playback player, int fadeTime = 5, float pan = 0.5f, float volume = 1f)
    {
        FadeTime = fadeTime;
        Pan = pan;
        Volume = volume;

        player.FadeTime = FadeTime;
        player.Pan = Pan;
        player.Volume = Volume;
    }
}