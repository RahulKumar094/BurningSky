using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameHUD gameHUD;
    public PowerUpPanel powerUpPanel;

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
}
