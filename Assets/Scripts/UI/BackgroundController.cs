using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BackgroundController : MonoBehaviour
{
    public VideoClip[] Backgrounds=new VideoClip[2];
    public GameObject[] backScenes = new GameObject[4];

    public void ChangeBackground(int i)
    {
        this.GetComponent<VideoPlayer>().clip=Backgrounds[i];
    }

    void Start()
    {
        backScenes[0] = GameObject.Find("00FlexTheBlock");
        backScenes[1] = GameObject.Find("01BlockTheBeach"); 
        backScenes[2] = GameObject.Find("02BlockFromHell"); 
        backScenes[3] = GameObject.Find("03RoadBlock"); 
        ChangeBackgroundScene(0);
    }

    public void ChangeBackgroundScene(int x)
    {
        for(int i = 0; i < backScenes.Length;i++)
        {
            if (i == x)
            {
                backScenes[i].SetActive(true);
            }
            else
            {
                backScenes[i].SetActive(false);
            }
        }


    }


}
