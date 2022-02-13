using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    public Slider HealthSlider;
    public TMP_Text HealthTxt;
    public TMP_Text CoinTxt;
    public TMP_Text ResumeCounterTxt;
    public TMP_Text LevelTxt;

    public void Initialize()
    {
        StartCoroutine("ShowLevelText");
        ResumeCounterTxt.gameObject.SetActive(false);
        HealthTxt.text = "100%";
        CoinTxt.text = "0%";
    }

    public void CoinCollected(float valueInPercent)
    {
        CoinTxt.text = Mathf.RoundToInt(valueInPercent) + "%"; 
    }

    public void SetHealth(float valueInPercent)
    {
        HealthSlider.value = valueInPercent / 100f;
        HealthTxt.text = Mathf.RoundToInt(valueInPercent) + "%";
    }

    public void OnClick_Pause()
    {
        UIManager.Instance.EnablePauseScreen(true);
    }

    public IEnumerator ResumeGame()
    {
        int remainingTime = 3;
        ResumeCounterTxt.gameObject.SetActive(true);

        while (remainingTime > 0)
        {
            ResumeCounterTxt.text = remainingTime.ToString();
            yield return new WaitForSeconds(1);
            remainingTime--;
        }

        ResumeCounterTxt.gameObject.SetActive(false);
    }

    private IEnumerator ShowLevelText()
    {
        LevelTxt.gameObject.SetActive(true);
        LevelTxt.text = "Level " + Game.Level;
        yield return new WaitForSeconds(2);
        LevelTxt.gameObject.SetActive(false);
    }
}
