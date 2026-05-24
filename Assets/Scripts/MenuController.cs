using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject gameSpawner;

    void Start()
    {
        Time.timeScale = 0f;
        menuPanel.SetActive(true);
        if (gameSpawner != null) gameSpawner.SetActive(false);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        if (gameSpawner != null) gameSpawner.SetActive(true);
    }
}