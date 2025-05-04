using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BackgroundController : MonoBehaviour
{
    public VideoClip[] Backgrounds=new VideoClip[2];


    public void ChangeBackground(int i)
    {
        this.GetComponent<VideoPlayer>().clip=Backgrounds[i];
    }

    void Start()
    {
        Backgrounds[0] = Resources.Load<VideoClip>("Backgrounds/FlameBeta");
        Backgrounds[1] = Resources.Load<VideoClip>("Backgrounds/Back1");
        ChangeBackground(1);
    }


}
