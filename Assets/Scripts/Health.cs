using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHP = 3;
    private int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log(gameObject.name + " HP: " + currentHP);

        if (currentHP <= 0)
        {
            if (CompareTag("Player"))
            {
                GameManager.PlayerDied();
            }
            Destroy(gameObject);
        }
    }
}