using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Canvas[] UIs = new Canvas[8];
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
        UIs[5] = GameObject.Find("InGameMenu").GetComponent<Canvas>();
        UIs[6] = GameObject.Find("CampaignMenu").GetComponent<Canvas>();
        UIs[7] = GameObject.Find("ChallengeMenu").GetComponent<Canvas>();
        ChangeMenu(0);
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
        if (TimeText != null)
        {
            TimeText.text = time.ToString("0.00");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
