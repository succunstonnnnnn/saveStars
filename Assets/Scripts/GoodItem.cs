using UnityEngine;

public enum HealType { Player, Sun }

public class GoodItem : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 direction = Vector2.down;
    public int healAmount = 1;
    public HealType healType = HealType.Player;

    private bool collected = false;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        float pulse = 1f + 0.1f * Mathf.Sin(Time.time * 3f);
        transform.localScale = initialScale * pulse;

        if (transform.position.y < -6f)
        {
            if (!collected)
            {
                collected = true;
                GameManager.GoodItemMissed();
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;
            GameManager.GoodItemCollected(healAmount, healType);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            collected = true;
            GameManager.GoodItemMissed();
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
