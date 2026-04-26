using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private static int score = 0;
    public static int targetScore = 10;
    private bool gameEnded = false;

    void Awake()
    {
        instance = this;
    }

    public static void AddScore()
    {
        if (instance.gameEnded) return;

        score++;
        Debug.Log("Killed astrophages: " + score + " / " + targetScore);

        if (score >= targetScore)
        {
            instance.EndGame("Star is saved!");
        }
    }

    public static void PlayerDied()
    {
        if (instance.gameEnded) return;
        instance.EndGame("Game over");
    }

    void EndGame(string message)
    {
        gameEnded = true;
        Debug.Log(message);
        Time.timeScale = 0f;
    }
}