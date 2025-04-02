using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Canvas[] UIs = new Canvas[5];
    private TMP_Text TimeText;


    // Start is called before the first frame update
    void Start()
    {
        TimeText = GameObject.Find("TimeText").GetComponent<TMP_Text>();

        UIs[0] = GameObject.Find("MainMenu").GetComponent<Canvas>();
        UIs[1] = GameObject.Find("SettingsMenu").GetComponent<Canvas>();
        UIs[2] = GameObject.Find("PlayMenu").GetComponent<Canvas>();
        UIs[3] = GameObject.Find("InGameUI").GetComponent<Canvas>();
        UIs[4] = GameObject.Find("ShopMenu").GetComponent<Canvas>();
        ChangeMenu(0);
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void ChangeMenu(int id)
    {
        for(int i = 0; i < UIs.Length; i++)
        {
            if (id == i)
            {
                UIs[i].enabled = true;
            }
            else
            {
                UIs[i].enabled = false;
            }


        }

    }

    //Wyœwietl czas
    public void UpdateTime(float time)
    {
        TimeText.text = time.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTime(1.58f);


    }
}
