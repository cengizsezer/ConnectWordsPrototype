using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sounds
{
    Intro = 0,
    Click = 1,
    LineDraw = 2,
    WordConnection = 3,
    Gold=4,
    LevelSuccess=5,
    Transition=6
}

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField] AudioSource audioSourceBG,audioSourceOneShot;

    [SerializeField] AudioClip[] clips;

    private void Start()
    {
        SetMusicEnabled(SaveLoadManager.IsMusicAvailable());
        SetSoundEnabled(SaveLoadManager.IsSoundAvailable());
    }

    public void PlayMusic()
    {
        if(!audioSourceBG.isPlaying)
            audioSourceBG.Play();
    }

    public void PlayOneShot(Sounds sound, float volume = 1f)
    {
        audioSourceOneShot.volume = volume;
        audioSourceOneShot.PlayOneShot(clips[(int)sound]);
    }

    public void SetMusicEnabled(bool isEnabled)
    {
        audioSourceBG.mute = !isEnabled;
    }

    public void SetSoundEnabled(bool isEnabled)
    {
        audioSourceOneShot.mute = !isEnabled;
    }

    public void SetPitch(float pitch)
    {
        audioSourceOneShot.pitch = pitch;
    }
}
