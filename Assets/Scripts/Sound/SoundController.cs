using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SoundController : MonoBehaviour
{
    public AudioMixer soundsMixer;
    public AudioMixer musicMixer;

    void Start()
    {
        soundsMixer = Resources.Load<AudioMixer>("Audio/MusicMixer");
        musicMixer = Resources.Load<AudioMixer>("Audio/SoundsMixer");

    }

    void FixedUpdate()
    {
        
    }
}

public class SoundsAssets
{
    #region Sounds
    //Klipy

    #endregion

    #region Music
    //Klipy

    #endregion

    public enum sound
    {
        //enumy düw
    }

    public enum music
    {
        //enumy muz
    }

    public AudioClip GetSound(sound soundName)
    {
        switch (soundName)
        {

        }
        return null;
    }

    public AudioClip GetMusic(music song)
    {
        switch (song)
        {

        }
        return null;
    }

}

