using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public Slider HealthSlider;
    public TMP_Text HealthTxt;
    public TMP_Text CoinTxt;

    public void Initialize()
    {
        HealthTxt.text = "100%";
        CoinTxt.text = "0%";
    }

    public void CoinCollected(float valueInPercent)
    {
        CoinTxt.text = Mathf.FloorToInt(valueInPercent) + "%"; 
    }

    public void SetHealth(float valueInPercent)
    {
        HealthSlider.value = valueInPercent / 100f;
        HealthTxt.text = valueInPercent + "%";
    }

    public void OnClick_Pause()
    {

    }
}
