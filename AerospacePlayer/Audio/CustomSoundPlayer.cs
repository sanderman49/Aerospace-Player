using SoundFlow.Abstracts;
using SoundFlow.Interfaces;

namespace SoundFlow.Components;

/// <summary>
/// A sound player that plays audio from a data provider.
/// </summary>
public sealed class CustomSoundPlayer(ISoundDataProvider dataProvider) : SoundPlayerBase(dataProvider)
{
    public bool Cancelled { get; set; }

    public float LinearVolume { get; set; } = 0f;
    /// <inheritdoc />
    public override string Name { get; set; } = "Custom Sound Player";

}