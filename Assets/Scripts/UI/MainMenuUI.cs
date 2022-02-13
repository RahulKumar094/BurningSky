using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public GameObject ButtonPanel;
    public GameObject SettingsPanel;
    public GameObject WelcomePanel;
    public GameObject HighScorePanel;
    public TMP_Text HighScoreTxt;
    public Slider Sensitivity;

    void OnEnable()
    {
        int hs = PlayerPrefs.GetInt(Game.Highscore_Key, -1);
        if (hs != -1)
        {
            HighScoreTxt.text = hs.ToString();
            HighScorePanel.SetActive(true);
            WelcomePanel.SetActive(false);
        }
        else
        {
            HighScorePanel.SetActive(false);
            WelcomePanel.SetActive(true);
        }

        ButtonPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    public void OnClick_Play()
    {
        Game.Level = 1;
        SceneLoader.LoadGameLevel();
    }

    public void OnClick_Settings()
    {
        Sensitivity.value = PlayerPrefs.GetFloat(Game.Sensitivity_Key, 0.5f);
        ButtonPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void OnClick_BackButton()
    {
        PlayerPrefs.SetFloat(Game.Sensitivity_Key, Sensitivity.value);
        ButtonPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }

    public void OnClick_Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
