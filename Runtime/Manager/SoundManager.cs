using UnityEngine;
using JoonyleGameDevKit;
using System.Collections.Generic;

public enum SfxType
{
    Landed = 0,
    Score = 5,
    Wrecked = 10,
    Clear = 15,
}

[System.Serializable]
public struct SfxEntry
{
    public SfxType type;
    public AudioClip clip;
}

public class SoundManager : Singleton<SoundManager>, IManager
{
    public int Priority => 10;

    [SerializeField] private SfxEntry[] _sfxEntries;

    private Dictionary<SfxType, SfxEntry> _sfxMap; // 캐싱
    private AudioSource _sfxSource;

    public void Initialize()
    {
        _sfxSource = GetComponent<AudioSource>();

        _sfxMap = new Dictionary<SfxType, SfxEntry>();

        foreach (var entry in _sfxEntries)
        {
            _sfxMap.TryAdd(entry.type, entry);
        }
    }

    public void PlaySfx(SfxType type, float volume = 0.5f)
    {
        if (_sfxMap.TryGetValue(type, out var entry) && entry.clip != null)
        {
            _sfxSource.PlayOneShot(entry.clip, volume);
        }
    }
}
