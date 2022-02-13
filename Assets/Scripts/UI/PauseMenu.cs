using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject ResumeBtn;
    public GameObject NextLevelBtn;

    public void OnClick_Resume()
    {
        UIManager.Instance.EnablePauseScreen(false);
    }

    public void OnClick_Restart()
    {
        gameObject.SetActive(false);
        GameManager.Instance.ResetLevel();
    }

    public void OnClick_MainMenu()
    {
        SceneLoader.LoadMainMenuScene();
    }

    public void OnClick_NextLevel()
    {
        gameObject.SetActive(false);
        GameManager.Instance.LoadNextLevel();
    }

    public void GamePaused()
    {
        ResumeBtn.SetActive(true);
        NextLevelBtn.SetActive(false);
    }

    public void LevelComplete()
    {
        ResumeBtn.SetActive(false);
        NextLevelBtn.SetActive(true);
    }

    public void GameOver()
    {
        ResumeBtn.SetActive(false);
        NextLevelBtn.SetActive(false);
    }
}
