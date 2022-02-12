using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpPanel : MonoBehaviour
{
    public Image MissileBG;
    public Image ShieldBG;

    public TMP_Text MissileCountTxt;
    public TMP_Text ShieldCountTxt;

    public void EnablePanel(bool enable)
    {
        gameObject.SetActive(enable);
    }

    public void UpdatePowerUp(float missileFillAmount, int missileCount, float shieldFillAmount, int shieldCount)
    {
        MissileBG.fillAmount = missileFillAmount;
        ShieldBG.fillAmount = shieldFillAmount;

        MissileCountTxt.text = "x" + missileCount;
        ShieldCountTxt.text = "x" + shieldCount;
    }

    public void OnClick_PowerUp_LaunchMissile()
    {
        GameManager.Instance.PlayerLaunchMissile();
    }

    public void OnClick_PowerUp_ActivateShield()
    {
        GameManager.Instance.PlayerActivateShield();
    }
}
