using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Canvas[] UIs = new Canvas[9];
    private TMP_Text TimeText;
    public bool[] Stars;
    public Image[] StarsIMG=new Image[3];
    public Button[] CampaignButtons=new Button[12];

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
        ChangeMenu(0);

        StarsIMG[0] = GameObject.Find("Star1").GetComponent<Image>();
        StarsIMG[1] = GameObject.Find("Star2").GetComponent<Image>();
        StarsIMG[2] = GameObject.Find("Star3").GetComponent<Image>();

        for(int i = 0; i < 12; i++)
        {
            CampaignButtons[i] = GameObject.Find("Lv" + (i + 1)).GetComponent<Button>();
        }

        LevelButtons(3);
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
        if (id == 3)
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
        for(int i = 0; i < 12; i++)
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

    public void StarWarning()
    {
        StarWarn = true;
    }

    public void LoseStar()
    {

    }

    public void EndGame(int stars)
    {
        ChangeMenu(8);
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
        str+= (int)time % 60;
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
        UpdateTime(1);
    }
}
