using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Audio[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("Music");
    }

    public void PlayMusic(string name)
    {
        Audio s = Array.Find(musicSounds, x => x.soundName == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            musicSource.PlayOneShot(s.clip);
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        Audio s = Array.Find(sfxSounds, x => x.soundName == name);

        if(s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void StopSFX(string name)
    {
        Audio s = Array.Find(sfxSounds, x => x.soundName == name);

        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            
            sfxSource.Stop();

        }
    }
}
