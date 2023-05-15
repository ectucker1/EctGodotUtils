using Godot;

/// <summary>
/// A proxy wrapper around an AudioStreamPlayer, AudioStreamPlayer2D, or AudioStreamPlayer3D.
/// </summary>
public partial class AudioStreamProxy : GodotObject
{
    private enum AudioStreamType
    {
        STREAM_NONE,
        STREAM_PLAYER,
        STREAM_PLAYER_2D,
        STREAM_PLAYER_3D,
        STREAM_COLLECTION
    }

    private AudioStreamType _type = AudioStreamType.STREAM_NONE;

    private AudioStreamPlayer _player;
    private AudioStreamPlayer2D _player2D;
    private AudioStreamPlayer3D _player3D;
    private IAudioCollection _collection;

    public bool Valid => _type != AudioStreamType.STREAM_NONE;

    public bool Playing
    {
        get
        {
            switch (_type)
            {
                case AudioStreamType.STREAM_PLAYER:
                    return _player.Playing;
                case AudioStreamType.STREAM_PLAYER_2D:
                    return _player2D.Playing;
                case AudioStreamType.STREAM_PLAYER_3D:
                    return _player3D.Playing;
                case AudioStreamType.STREAM_COLLECTION:
                    return _collection.Playing;
                default:
                    return false;
            }
        }
    }

    public float PitchScale
    {
        get
        {
            switch (_type)
            {
                case AudioStreamType.STREAM_PLAYER:
                    return _player.PitchScale;
                case AudioStreamType.STREAM_PLAYER_2D:
                    return _player2D.PitchScale;
                case AudioStreamType.STREAM_PLAYER_3D:
                    return _player3D.PitchScale;
                default:
                    return 1.0f;
            }
        }
        set
        {
            switch (_type)
            {
                case AudioStreamType.STREAM_PLAYER:
                    _player.PitchScale = value;
                    break;
                case AudioStreamType.STREAM_PLAYER_2D:
                    _player2D.PitchScale = value;
                    break;
                case AudioStreamType.STREAM_PLAYER_3D:
                    _player3D.PitchScale = value;
                    break;
            }
        }
    }

    public float VolumeDb
    {
        get
        {
            switch (_type)
            {
                case AudioStreamType.STREAM_PLAYER:
                    return _player.VolumeDb;
                case AudioStreamType.STREAM_PLAYER_2D:
                    return _player2D.VolumeDb;
                case AudioStreamType.STREAM_PLAYER_3D:
                    return _player3D.VolumeDb;
                default:
                    return 0.0f;
            }
        }
        set
        {
            switch (_type)
            {
                case AudioStreamType.STREAM_PLAYER:
                    _player.VolumeDb = value;
                    break;
                case AudioStreamType.STREAM_PLAYER_2D:
                    _player2D.VolumeDb = value;
                    break;
                case AudioStreamType.STREAM_PLAYER_3D:
                    _player3D.VolumeDb = value;
                    break;
            }
        }
    }
    
    [Signal]
    public delegate void FinishedEventHandler();

    public AudioStreamProxy(AudioStreamPlayer player)
    {
        _type = AudioStreamType.STREAM_PLAYER;
        this._player = player;
        this._player.Finished += EmitFinished;
    }

    public AudioStreamProxy(AudioStreamPlayer2D player)
    {
        _type = AudioStreamType.STREAM_PLAYER_2D;
        this._player2D = player;
        this._player2D.Finished += EmitFinished;
    }

    public AudioStreamProxy(AudioStreamPlayer3D player)
    {
        _type = AudioStreamType.STREAM_PLAYER_3D;
        this._player3D = player;
        this._player3D.Finished += EmitFinished;
    }
    
    private void EmitFinished()
    {
        EmitSignal(nameof(Finished));
    }

    public AudioStreamProxy(Node other)
    {
        if (other is AudioStreamPlayer player)
        {
            _type = AudioStreamType.STREAM_PLAYER;
            this._player = player;
            this._player.Finished += () => EmitSignal(SignalName.Finished);
        }
        else if (other is AudioStreamPlayer2D player2D)
        {
            _type = AudioStreamType.STREAM_PLAYER_2D;
            this._player2D = player2D;
            this._player2D.Finished += () => EmitSignal(SignalName.Finished);
        }
        else if (other is AudioStreamPlayer3D player3D)
        {
            _type = AudioStreamType.STREAM_PLAYER_3D;
            this._player3D = player3D;
            this._player3D.Finished += () => EmitSignal(SignalName.Finished);
        }
        else if (other is IAudioCollection collection)
        {
            _type = AudioStreamType.STREAM_COLLECTION;
            this._collection = collection;
            this._collection.ConnectFinished(new Callable(this, nameof(EmitFinished)));
        }
    }

    public void Play(float from = 0.0f)
    {
        switch (_type)
        {
            case AudioStreamType.STREAM_PLAYER:
                _player.Play(from);
                break;
            case AudioStreamType.STREAM_PLAYER_2D:
                _player2D.Play(from);
                break;
            case AudioStreamType.STREAM_PLAYER_3D:
                _player3D.Play(from);
                break;
            case AudioStreamType.STREAM_COLLECTION:
                _collection.Play(from);
                break;
        }
    }

    public void Stop()
    {
        switch (_type)
        {
            case AudioStreamType.STREAM_PLAYER:
                _player.Stop();
                break;
            case AudioStreamType.STREAM_PLAYER_2D:
                _player2D.Stop();
                break;
            case AudioStreamType.STREAM_PLAYER_3D:
                _player3D.Stop();
                break;
            case AudioStreamType.STREAM_COLLECTION:
                _collection.Stop();
                break;
        }
    }
}
