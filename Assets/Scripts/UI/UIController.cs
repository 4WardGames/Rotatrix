using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Canvas[] UIs = new Canvas[9];
    private TMP_Text TimeText;
    public bool[] Stars;
    public Image[] StarsIMG = new Image[3];
    public Button[] CampaignButtons = new Button[12];


    public bool StarWarn;
    public float ClockCounter;

    // Start is called before the first frame update
    void Start()
    {
        TimeText = GameObject.Find("TimeText").GetComponent<TMP_Text>();

        UIs[0] = GameObject.Find("MainMenu").GetComponent<Canvas>();
        UIs[1] = GameObject.Find("SettingsMenu").GetComponent<Canvas>();
        UIs[2] = GameObject.Find("PlayMenu").GetComponent<Canvas>();
        UIs[3] = GameObject.Find("InGameUI").GetComponent<Canvas>();
        UIs[4] = GameObject.Find("ShopMenu").GetComponent<Canvas>();
        UIs[5] = GameObject.Find("InGameMenu").GetComponent<Canvas>();
        UIs[6] = GameObject.Find("CampaignMenu").GetComponent<Canvas>();
        UIs[7] = GameObject.Find("ChallengeMenu").GetComponent<Canvas>();
        UIs[8] = GameObject.Find("GameOverMenu").GetComponent<Canvas>();
        Stars = new bool[3] { true, true, true };
        ChangeMenu(0);


        StarsIMG[0] = GameObject.Find("Star1").GetComponent<Image>();
        StarsIMG[1] = GameObject.Find("Star2").GetComponent<Image>();
        StarsIMG[2] = GameObject.Find("Star3").GetComponent<Image>();

        for (int i = 0; i < 12; i++)
        {
            CampaignButtons[i] = GameObject.Find("Lv" + (i + 1)).GetComponent<Button>();
        }

        //UpdatePlayerStars(0);
        UpdatePlayerStars(0);
        SetStars(1);
        EndGame();
    }

    public void NewLevel()
    {
        Stars = new bool[] { true, true, true };
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void ChangeMenu(int id)
    {
        if (id == 3 || id == 8)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }

        for (int i = 0; i < UIs.Length; i++)
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

    public void LevelButtons(int u)
    {
        for (int i = 0; i < 12; i++)
        {
            if (i < u)
            {
                CampaignButtons[i].interactable = true;
            }

            else
            {
                CampaignButtons[i].interactable = false;
            }
        }

    }

    public void UpdatePlayerStars(int stars)
    {
        GameObject.Find("StarCountTxt").GetComponent<TMP_Text>().text = stars.ToString();
        LevelButtons(stars + 1);
    }

    public void StarWarning()
    {
        //Nie u¿ywaæ
        StarWarn = true;
    }

    public void SetStars(int stars)
    {
        for (int i = stars; i < 3; i++)
        {
            Stars[i] = false;
            StarsIMG[i].color = Color.black;
        }
    }

    public void LoseStar()
    {
        //Wywo³aæ przy stracie gwiazdki
        for (int i = 0; i < 3; i++)
        {
            if (Stars[2 - i] == true)
            {
                Stars[2 - i] = false;
                StarsIMG[2 - i].color = Color.black;
                break;
            }
        }
    }

    public void NewGame()
    {
        for (int i = 0; i < 3; i++)
        {
            Stars[i] = true;
            StarsIMG[i].color = Color.white;



        }
    }

    public void EndGame()
    {
        int n = 0;
        for (int i = 0; i < 3; i++)
        {
            GameObject.Find("EndStar" + (i + 1)).GetComponent<Image>().color = Color.black;
            if (Stars[i] == true)
            {
                n++;
            }
        }
        Time.timeScale = 1;
        ChangeMenu(8);
        StartCoroutine(StarsAnim(n));
    }

    IEnumerator StarsAnim(int n)
    {
        for (int i = 0; i < n; i++)
        {
            yield return new WaitForSeconds(0.5f);
            StarsIMG[i].color = Color.white;
            GameObject.Find("EndStar" + (i + 1)).GetComponent<Image>().color = Color.white;
            GameObject.Find("EndStar" + (i + 1)).GetComponent<Animator>().Play("Anim");
        }
    }

    //SETTINGS
    public void ChangeLanguage(int id)
    {
        //Empty
    }

    public void ChangeMusicVolume(float volume)
    {
        //Empty
    }

    public void ChangeSoundsVolume(float volume)
    {
        //Empty
    }

    //Wyœwietl czas
    public void UpdateTime(float time)
    {
        string str = ((int)time / 60).ToString();
        if (time % 60 < 10)
        {
            str += ":0";
        }
        else
        {
            str += ":";
        }
        str += (int)time % 60;
        TimeText.text = str;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (StarWarn)
        {
            ClockCounter += 0.02f;
            if (ClockCounter >= 1.5f)
            {
                ClockCounter = 0;
            }
            float temp = Mathf.Sin(ClockCounter * Mathf.PI / 1.5f);
            Debug.Log(temp);
            if (Stars[2] == true)
            {
                StarsIMG[2].color = new Color(temp, temp, temp, 1);
            }
        }
    }
}
