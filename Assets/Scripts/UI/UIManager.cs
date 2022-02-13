using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameHUD gameHUD;
    public PowerUpPanel powerUpPanel;
    public PauseMenu pauseMenu;

    public static UIManager Instance { get { return instance; } }
    private static UIManager instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Initialize()
    {
        gameHUD.Initialize();
    }

    public void EnablePowerUpPanel(bool enable)
    {
        powerUpPanel.EnablePanel(enable);
    }

    public void UpdatePowerUp(float missileFillAmount, int missileCount, float shieldFillAmount, int shieldCount)
    {
        powerUpPanel.UpdatePowerUp(missileFillAmount, missileCount, shieldFillAmount, shieldCount);
    }

    public void SetHealth(float valueInPercent)
    {
        gameHUD.SetHealth(valueInPercent);
    }

    public void SetCoinText(float valueInPercent)
    {
        gameHUD.CoinCollected(valueInPercent);
    }

    public void GameOver()
    {
        EnablePauseScreen(true);
        pauseMenu.DisableResumeButton();
    }

    public void EnablePauseScreen(bool enable)
    {
        pauseMenu.gameObject.SetActive(enable);
        if (enable)
            GameManager.Instance.GamePaused();
        else
            StartCoroutine("ResumeGame");
    }

    private IEnumerator ResumeGame()
    {
        yield return StartCoroutine(gameHUD.ResumeGame());
        GameManager.Instance.ResumeGame();
    }
}
