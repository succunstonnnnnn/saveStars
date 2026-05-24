using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 3;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        UpdateUI();

        if (currentHP <= 0)
        {
            if (CompareTag("Player"))
                GameManager.PlayerDied();
            else if (CompareTag("Sun"))
                GameManager.SunDied();

            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (UIManager.instance == null) return;

        if (CompareTag("Player"))
            UIManager.instance.UpdatePlayerHP(currentHP, maxHP);
        else if (CompareTag("Sun"))
            UIManager.instance.UpdateSunHP(currentHP, maxHP);
    }
}
