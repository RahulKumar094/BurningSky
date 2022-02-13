using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject ResumeBtn;
    public void OnClick_Resume()
    {
        UIManager.Instance.EnablePauseScreen(false);
    }

    public void OnClick_Restart()
    {
        gameObject.SetActive(false);
        GameManager.Instance.RestartLevel();
    }

    public void OnClick_MainMenu()
    {
        SceneLoader.LoadMainMenuScene();
    }

    public void DisableResumeButton()
    {
        ResumeBtn.SetActive(false);
    }
}
