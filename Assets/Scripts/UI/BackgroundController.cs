using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BackgroundController : MonoBehaviour
{
    public VideoClip[] Backgrounds=new VideoClip[2];
    [SerializeField]
    public GameObject[] backScenes = new GameObject[4];

    public void ChangeBackground(int i)
    {
        this.GetComponent<VideoPlayer>().clip=Backgrounds[i];
    }

    void Start()
    {
        backScenes = new GameObject[4];
        backScenes[0] = transform.Find("00FlexTheBlock").gameObject;
        backScenes[1] = transform.Find("01BlockTheBeach").gameObject;
        backScenes[2] = transform.Find("02BlockFromHell").gameObject; 
        backScenes[3] = transform.Find("03RoadBlock").gameObject; 
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
