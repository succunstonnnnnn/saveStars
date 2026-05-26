using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject aboutPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ShowAbout()
    {
        if (aboutPanel != null)
            aboutPanel.SetActive(true);
    }

    public void CloseAbout()
    {
        if (aboutPanel != null)
            aboutPanel.SetActive(false);
    }
}
