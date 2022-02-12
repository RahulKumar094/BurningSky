using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public GameObject WelcomePanel;
    public GameObject HighScorePanel;
    public TMP_Text HighScoreTxt;

    public const string HIGHSCOREKEY = "High_Score_Key";

    void OnEnable()
    {
        int hs = PlayerPrefs.GetInt(HIGHSCOREKEY, -1);
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
    }

    public void OnClick_Play()
    {
        Game.NextSceneToLoad = SceneNames.Level_1;
        SceneManager.LoadScene(SceneNames.Loading);
    }

    public void OnClick_Settings()
    {
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
