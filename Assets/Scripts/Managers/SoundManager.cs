using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Singleton;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoGenericSingleton<SoundManager>
{
    [SerializeField]
    // private AudioSource gemBreakSound, explodeSound, stoneBreakSound, roundOverSound;

    public AudioSource soundEffect;
    public AudioSource soundMusic;
    public SoundType[] sounds;
    
    public void Play(Sounds sound)
    {
        AudioClip clip = GetSoundClip(sound);
        if (clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("Clip not found for sound type: " + sound);
        }
    }

    private AudioClip GetSoundClip(Sounds sound)
    {
        SoundType soundType = Array.Find(sounds, item => item.soundType == sound);
        if (soundType != null)
            return soundType.soundClip;
        return null;

    }
}

[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}
