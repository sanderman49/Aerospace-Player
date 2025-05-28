
using System;
using System.IO;
using SoundFlow.Abstracts;
using SoundFlow.Enums;
using SoundFlow.Interfaces;

namespace aeropad_player.Audio;

public sealed class SimpleSoundPlayer(ISoundDataProvider dataProvider) : SoundComponent
{
    private readonly ISoundDataProvider _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
    private PlaybackState _state = PlaybackState.Stopped;
    private int _samplePosition;

    public PlaybackState State => _state;
    public bool IsLooping { get; set; }
    public float PlaybackSpeed { get; set; } = 1.0f;
    public float Time => (float)_samplePosition / AudioEngine.Channels / AudioEngine.Instance.SampleRate;
    public float Duration => (float)_dataProvider.Length / AudioEngine.Channels / AudioEngine.Instance.SampleRate;
    public int LoopStartSamples => 0;
    public int LoopEndSamples => -1;
    public float LoopStartSeconds => 0f;
    public float LoopEndSeconds => -1f;
    public bool cancelled = false;


    public event EventHandler<EventArgs>? PlaybackEnded;

    protected override void GenerateAudio(Span<float> output)
    {
        if (_state != PlaybackState.Playing)
        {
            output.Clear(); // Ensure silence when not playing
            return;
        }

        int samplesRead = _dataProvider.ReadBytes(output);
        _samplePosition += samplesRead;

        if (samplesRead == 0)
        {
            _state = PlaybackState.Stopped;
            PlaybackEnded?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Play()
    {
        _state = PlaybackState.Playing;
        Enabled = true;
    }

    public void Pause()
    {
        _state = PlaybackState.Paused;
        Enabled = false;
    }

    public void Stop()
    {
        _state = PlaybackState.Stopped;
        Enabled = false;
        Seek(0);
    }

    public bool Seek(TimeSpan time, SeekOrigin seekOrigin = SeekOrigin.Begin)
    {
        return Seek((float)time.TotalSeconds);
    }

    public bool Seek(float time)
    {
        if (Duration <= 0) return Seek(0);
        var sampleOffset = (int)(time / Duration * _dataProvider.Length);
        return Seek(sampleOffset);
    }

    public bool Seek(int sampleOffset)
    {
        if (!_dataProvider.CanSeek) return false;
        _dataProvider.Seek(sampleOffset);
        _samplePosition = sampleOffset;
        return true;
    }

    public void SetLoopPoints(float startTime, float? endTime = -1f) { }
    public void SetLoopPoints(int startSample, int endSample = -1) { }
    public void SetLoopPoints(TimeSpan startTime, TimeSpan? endTime = null) { }
}