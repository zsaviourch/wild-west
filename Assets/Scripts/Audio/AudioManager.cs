using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioType
{
    BGM,
    SFX
}

[System.Serializable]
public class AudioData
{
    public string name;
    public AudioClip clip;
    public AudioType type;
    public bool loop;
    public float volume;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioData[] bgmData;
    [SerializeField] private AudioData[] sfxData;

    private Dictionary<string, AudioSource> bgmSources = new Dictionary<string, AudioSource>();
    private Dictionary<string, AudioSource> sfxSources = new Dictionary<string, AudioSource>();

    public float masterBGMVolume = 1f;
    public float masterSFXVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Create audio sources for BGM
        foreach (var data in bgmData)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = data.clip;
            source.loop = data.loop;
            source.volume = data.volume;
            bgmSources[data.name] = source;
        }

        // Create audio sources for SFX
        foreach (var data in sfxData)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.clip = data.clip;
            source.loop = data.loop;
            source.volume = data.volume;
            sfxSources[data.name] = source;
        }
    }

    public void Play(string name)
    {
        if (bgmSources.TryGetValue(name, out AudioSource source))
        {
            SetVolume(name, source.volume * masterBGMVolume);
            source.Play();
        }
        else if (sfxSources.TryGetValue(name, out source))
        {
            Debug.Log("Playing SFX" + name);
            SetVolume(name, source.volume * masterSFXVolume);
            source.Play();
        }
    }

    public void Stop(string name)
    {
        if (bgmSources.TryGetValue(name, out AudioSource source))
        {
            source.Stop();
        }
        else if (sfxSources.TryGetValue(name, out source))
        {
            source.Stop();
        }
    }

    public void SetVolume(string name, float volume)
    {
        if (bgmSources.TryGetValue(name, out AudioSource source))
        {
            source.volume = volume;
        }
        else if (sfxSources.TryGetValue(name, out source))
        {
            source.volume = volume;
        }
    }
}
