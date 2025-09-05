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
    public AudioSource Music;


    void Start()
    {
        Music = GameObject.Find("Music").GetComponent<AudioSource>();
        soundsMixer = Resources.Load<AudioMixer>("Audio/SoundsMixer");
        musicMixer = Resources.Load<AudioMixer>("Audio/MusicMixer");
        Assets.LoadResources();
        PlaySound(SoundsAssets.sound.sndSplit);
        ChangeMusicVolume(8);
        ChangeSoundVolume(8);
    }

    public void PlaySound(SoundsAssets.sound sound)
    {
        this.transform.GetComponent<AudioSource>().PlayOneShot(Assets.GetSound(sound));
    }

    public void PlaySplit()
    {
        PlaySound(SoundsAssets.sound.sndSplit);
    }

    public void PlayCombine()
    {
        PlaySound(SoundsAssets.sound.sndCombine);
    }

    public void ButtonClickSound()
    {
        this.transform.GetComponent<AudioSource>().PlayOneShot(Assets.GetSound(SoundsAssets.sound.sndBtn));
    }

    public void ChangeMusicVolume(float volume)
    {
        musicMixer.SetFloat("MasterVolume", -50f + 5 * volume);
    }

    public void ChangeSoundVolume(float volume)
    {
        soundsMixer.SetFloat("MasterVolume", -50f + 5 * volume);
    }

    void Update()
    {
        if (Music.clip.length<=Music.time)
        {
            int r = Random.Range(0, 3);
            switch (r)
            {
                case 0:
                Music.clip = Assets.GetMusic(SoundsAssets.music.MainEvent);
                    break;
                case 1:
                    Music.clip = Assets.GetMusic(SoundsAssets.music.Block_by_block);
                    break;
                case 2:
                    Music.clip = Assets.GetMusic(SoundsAssets.music.Blocked);
                    break;
            }
            Music.Play();
        }
    }
}

public class SoundsAssets
{
    #region Sounds
    AudioClip[] Sounds = new AudioClip[3];

    #endregion

    #region Music
    AudioClip[] Music = new AudioClip[3];

    #endregion

    public enum sound
    {
        sndBtn,
        sndCombine,
        sndSplit,
    }

    public enum music
    {
        MainEvent,
        Block_by_block,
        Blocked,
        //enumy muz
    }

    public void LoadResources()
    {
        Sounds[0] = Resources.Load<AudioClip>("Audio/Sounds/Button");
        Sounds[1] = Resources.Load<AudioClip>("Audio/Sounds/Comb");
        Sounds[2] = Resources.Load<AudioClip>("Audio/Sounds/Sep");

        Music[0] = Resources.Load<AudioClip>("Audio/Music/0-MainEvent");
        Music[1] = Resources.Load<AudioClip>("Audio/Music/01-Block_by_block");
        Music[2] = Resources.Load<AudioClip>("Audio/Music/02-Blocked");
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
            case music.MainEvent:
                return Music[0];
            case music.Block_by_block:
                return Music[1];
            case music.Blocked:
                return Music[2];
        }
        return null;
    }

}

