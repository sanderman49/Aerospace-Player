using SoundFlow.Abstracts;
using SoundFlow.Backends.MiniAudio;
using SoundFlow.Interfaces;
using SoundFlow.Structs;

namespace SoundFlow.Components;

/// <summary>
/// A sound player that plays audio from a data provider.
/// </summary>
public sealed class CustomSoundPlayer(AudioEngine engine, AudioFormat format, ISoundDataProvider dataProvider) : SoundPlayerBase(engine, format, dataProvider)
{
    public bool Cancelled { get; set; }
    public float LinearVolume { get; set; } = 0f;
    
    /// <inheritdoc />
    public override string Name { get; set; } = "Custom Sound Player";
}