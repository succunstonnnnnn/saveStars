using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int maxMissedEnemies = 3;
    public int maxMissedGoodItems = 3;
    public GameObject sun;

    private int missedEnemies = 0;
    private int missedGoodItems = 0;
    private int collectedItems = 0;
    private int enemiesKilled = 0;
    private bool gameEnded = false;
    private int highScore = 0;

    void Awake()
    {
        instance = this;
        ScoreDatabase.Initialize();
        highScore = ScoreDatabase.GetHighScore();
    }

    void Start()
    {
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateMissedEnemies(0, maxMissedEnemies);
            UIManager.instance.UpdateKilledEnemies(0);
        }
    }

    public static void EnemyKilled()
    {
        if (instance == null || instance.gameEnded) return;
        instance.enemiesKilled++;

        if (UIManager.instance != null)
            UIManager.instance.UpdateKilledEnemies(instance.enemiesKilled);
    }

    public static void EnemyMissed()
    {
        if (instance == null || instance.gameEnded) return;
        instance.missedEnemies++;

        if (UIManager.instance != null)
            UIManager.instance.UpdateMissedEnemies(instance.missedEnemies, instance.maxMissedEnemies);

        if (instance.missedEnemies >= instance.maxMissedEnemies)
            instance.EndGame("Game Over: Too many astrofages passed");
    }

    public static void GoodItemMissed()
    {
        if (instance == null || instance.gameEnded) return;
        instance.missedGoodItems++;
    }

    public static void GoodItemCollected(int healAmount, HealType healType)
    {
        if (instance == null || instance.gameEnded) return;
        instance.collectedItems++;

        if (healType == HealType.Player)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                    playerHealth.Heal(healAmount);
            }
        }
        else if (healType == HealType.Sun)
        {
            if (instance.sun != null)
            {
                Health sunHealth = instance.sun.GetComponent<Health>();
                if (sunHealth != null)
                    sunHealth.Heal(healAmount);
            }
        }
    }

    public static void PlayerDied()
    {
        if (instance == null || instance.gameEnded) return;
        instance.EndGame("Game Over: Astronaut is dead");
    }

    public static void SunDied()
    {
        if (instance == null || instance.gameEnded) return;
        instance.EndGame("Game Over: Sun has faded");
    }

    void EndGame(string message)
    {
        gameEnded = true;
        if (enemiesKilled > highScore)
            highScore = enemiesKilled;
        ScoreDatabase.SaveScore(enemiesKilled);
        if (UIManager.instance != null)
            UIManager.instance.ShowEndGame(message, enemiesKilled, highScore);
        Time.timeScale = 0f;
    }
}
