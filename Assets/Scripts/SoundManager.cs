using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume;
    [Range(0.1f, 3f)] public float pitch;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public List<Sound> sounds = new List<Sound>();

    public AudioSource audioSource;
    private Sound backGroundSound;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);

        backGroundSound = FindSound("Background");
        StartBackgroundMusic();
    }

    private void StartBackgroundMusic()
    {
        audioSource.clip = backGroundSound.clip;
        audioSource.pitch = backGroundSound.pitch;
        audioSource.loop = true;
        audioSource.volume = PlayerPrefs.GetFloat("Volume", 1f);
        audioSource.Play();
    }

    public void PlayMusic()
    {
        audioSource.UnPause();
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }
    
    public void PlayClip(string name)
    {
        if (PlayerPrefs.GetInt("Music", 1) == 0)
            return;
        Sound sound = FindSound(name);
        audioSource.pitch = sound.pitch;
        audioSource.PlayOneShot(sound.clip);
    }

    private Sound FindSound(string name) => sounds.Find(sound => sound.name == name);
}