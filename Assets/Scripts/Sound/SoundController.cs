using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using static SoundsAssets;


public class SoundController : MonoBehaviour
{
    SoundsAssets Assets=new SoundsAssets();

    public AudioMixer soundsMixer;
    public AudioMixer musicMixer;


    void Start()
    {
        soundsMixer = Resources.Load<AudioMixer>("Audio/MusicMixer");
        musicMixer = Resources.Load<AudioMixer>("Audio/SoundsMixer");
        Assets.LoadResources();
        PlaySound(SoundsAssets.sound.sndSplit);
    }

    public void PlaySound(SoundsAssets.sound sound)
    {
        this.transform.GetComponent<AudioSource>().PlayOneShot(Assets.GetSound(sound));
    }

    public void ButtonClickSound()
    {
        this.transform.GetComponent<AudioSource>().PlayOneShot(Assets.GetSound(SoundsAssets.sound.sndBtn));
    }

    void FixedUpdate()
    {
        
    }
}

public class SoundsAssets
{
    #region Sounds
    AudioClip[] Sounds = new AudioClip[3];

    #endregion

    #region Music
    AudioClip[] Musicc = new AudioClip[3];

    #endregion

    public enum sound
    {
        sndBtn,
        sndCombine,
        sndSplit,
    }

    public enum music
    {
        //enumy muz
    }

    public void LoadResources()
    {
        Sounds[0] = Resources.Load<AudioClip>("Audio/Sounds/Button");
        Sounds[1] = Resources.Load<AudioClip>("Audio/Sounds/Comb");
        Sounds[2] = Resources.Load<AudioClip>("Audio/Sounds/Sep");
    }

    public AudioClip GetSound(sound soundName)
    {
        switch (soundName)
        {
            case sound.sndBtn:
                return Sounds[0];

            case sound.sndCombine:
                return Sounds[1];

            case sound.sndSplit:
                return Sounds[2];
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

