using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Canvas[] UIs = new Canvas[9];
    private TMP_Text TimeText;
    public GameObject TutorialText;
    public bool[] Stars;
    public Image[] StarsIMG = new Image[3];
    public Button[,] CampaignButtons = new Button[12, 3];
    public int SelCampaign = 0;
    public Canvas[] Campaigns = new Canvas[3];

    //UI Moves
    public TMP_Text MinimumMoves;
    public TMP_Text MinimumTime;

    public bool StarWarn;
    public float ClockCounter;

    GameObject camera;
    GameObject MenuParticles;

    BackgroundController Background;

    // Start is called before the first frame update
    void Start()
    {
        MenuParticles = GameObject.Find("MenuParticles");
        camera = GameObject.Find("Main Camera");
        TimeText = GameObject.Find("TimeText").GetComponent<TMP_Text>();
        TutorialText = GameObject.Find("TutorialText");
        TutorialText.SetActive(false);

        MinimumMoves = GameObject.Find("MiminumMovesTxt").GetComponent<TMP_Text>();
        MinimumTime = GameObject.Find("MinimumTimeTxt").GetComponent<TMP_Text>();

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

        StarsIMG[0] = GameObject.Find("Star1").GetComponent<Image>();
        StarsIMG[1] = GameObject.Find("Star2").GetComponent<Image>();
        StarsIMG[2] = GameObject.Find("Star3").GetComponent<Image>();
        ChangeMenu(0);

        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if(GameObject.Find(j + "Lv" + (i + 1)) != null)
                {
                    CampaignButtons[i, j] = GameObject.Find(j + "Lv" + (i + 1)).GetComponent<Button>();
                }
            }
        }
        for (int i = 0; i < 3; i++)
        {
            Campaigns[i] = GameObject.Find("Cmp" + i).GetComponent<Canvas>();
            Campaigns[i].enabled = false;
        }
        Campaigns[0].enabled = true;
        SaveController.LoadLevelStars();
        SetLevelButtons();
        UpdatePlayerStars(0);
        SetStars(1);
        Background = GameObject.Find("BackgroundScenes").GetComponent<BackgroundController>();
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
        if (id == 3 || id == 8||id==0)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }

        if (id == 0)
        {
            camera.transform.rotation = Quaternion.Euler(-40, 180, 0);
            MenuParticles.SetActive(true);
        }
        else
        {
            camera.transform.rotation = Quaternion.Euler(0, 0, 0);
            MenuParticles.SetActive(false);
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
            for (int j = 0; j < 3; j++)
            {
                if (i < u)
                {
                    CampaignButtons[i, j].interactable = true;
                }

                else
                {
                    CampaignButtons[i, j].interactable = false;
                }
            }
        }

    }

    public void CheckNextLevelButton(int currentLevel)
    {
        var stars = 0;
        var levelStars = SaveController.levels;

        foreach (var level in levelStars.stars)
        {
            stars += level;
        }

        if ((currentLevel + 1) * 2 <= stars)
        {
            GameObject.Find("NextLvl").GetComponent<Button>().interactable = true;
        }
        else
        {
            GameObject.Find("NextLvl").GetComponent<Button>().interactable = false;
        }


    }

    public void UpdatePlayerStars(int stars)
    {
        SetLevelButtons();
    }

    public void SetLevelButtons()
    {
        var stars = 0;
        var levelStars = SaveController.levels;

        foreach (var level in levelStars.stars)
        {
            stars += level;
        }
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 12; i++)
            {
                if (i + (j * 12) <= stars / 2)
                {
                    if(CampaignButtons[i, j] != null)
                    {
                        CampaignButtons[i, j].interactable = true;
                    }
                }

                else
                {
                    if (CampaignButtons[i, j] != null)
                    {
                        CampaignButtons[i, j].interactable = false;
                    }
                }
            }
        }
        GameObject.Find("StarCountTxt").GetComponent<TMP_Text>().text = stars.ToString();
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

    public void LoseStarInfo(string moves, string time)
    {
        MinimumMoves.text = moves;
        MinimumTime.text = time;
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
        TutorialText.SetActive(false);
        ChangeMenu(8);
        StartCoroutine(StarsAnim(n));
    }

    public void ChangeCampaign(int delta)
    {
        Campaigns[SelCampaign].enabled = false;
        SelCampaign += delta;

        if (SelCampaign < 0)
        {
            SelCampaign = 0;
        }
        else if (SelCampaign > 2)
        {
            SelCampaign = 2;
        }
        Campaigns[SelCampaign].enabled = true;
        Background.ChangeBackgroundScene(SelCampaign);
    }

    public void UpdateLevelStars(int campaign, int level, int stars)
    {
        if (GameObject.Find(campaign + "Lv" + level) != null)
        {
            for (int i = 0; i < stars; i++)
            {
                GameObject.Find(campaign + "Lv" + level).transform.Find("Stars").transform.Find("CS" + (i + 1)).GetComponent<Image>().enabled = true;
                
            }
        }
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

    public void SetTutorialText(string text)
    {
        TutorialText.SetActive(true);
        TutorialText.GetComponent<TMP_Text>().text = text;
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
