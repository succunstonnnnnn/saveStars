using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Slider playerHPSlider;
    public Slider sunHPSlider;
    public TMP_Text killedEnemyText;
    public TMP_Text missedEnemyText;
    public TMP_Text missedGoodText;
    public GameObject endGameTextObject;
    public TMP_Text endGameText;
    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    void Awake()
    {
        instance = this;
    }

    public void UpdatePlayerHP(int current, int max)
    {
        if (playerHPSlider == null) return;
        playerHPSlider.maxValue = max;
        playerHPSlider.value = current;
        if (playerHPSlider.fillRect != null)
        {
            Image fill = playerHPSlider.fillRect.GetComponent<Image>();
            if (fill != null)
                fill.color = current <= 1 ? Color.red : Color.green;
        }
    }

    public void UpdateSunHP(int current, int max)
    {
        if (sunHPSlider == null) return;
        sunHPSlider.maxValue = max;
        sunHPSlider.value = current;
        if (sunHPSlider.fillRect != null)
        {
            Image fill = sunHPSlider.fillRect.GetComponent<Image>();
            if (fill != null)
                fill.color = current <= 1 ? Color.red : Color.green;
        }
    }

    public void UpdateKilledEnemies(int current)
    {
        if (killedEnemyText != null)
            killedEnemyText.text = "Destroyed: " + current;
    }

    public void UpdateMissedEnemies(int current, int max)
    {
        if (missedEnemyText != null)
            missedEnemyText.text = "Missed Astrophages: " + current + "/" + max;
    }

    public void ShowEndGame(string message, int score, int highScore)
    {
        if (endGameTextObject != null)
            endGameTextObject.SetActive(true);

        TMP_Text target = endGameText;
        if (target == null && endGameTextObject != null)
            target = endGameTextObject.GetComponentInChildren<TMP_Text>();

        if (target != null)
        {
            target.color = Color.red;

            int sep = message.IndexOf(": ");
            string title  = sep >= 0 ? message.Substring(0, sep) : message;
            string reason = sep >= 0 ? message.Substring(sep + 2) : "";

            string text = title;
            if (reason.Length > 0)
                text += "\n<size=55%>" + reason + "</size>";
            text += "\n<size=40%><color=white>Your score: " + score + " </color></size>";
            text += "\n<size=40%><color=white>Best score: " + highScore + "</color></size>";
            target.text = text;
        }

        if (scoreText != null)
            scoreText.text = "Your score: " + score + " ";
        if (highScoreText != null)
            highScoreText.text = "Best score: " + highScore;
    }
}
