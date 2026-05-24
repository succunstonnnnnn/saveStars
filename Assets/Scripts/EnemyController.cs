using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 2f;
    public int damage = 1;
    public Vector2 direction = Vector2.down;
    public float speedMultiplier = 1f;

    private Transform sunTransform;

    void Start()
    {
        GameObject sun = GameObject.FindWithTag("Sun");
        if (sun != null)
            sunTransform = sun.transform;
    }

    void Update()
    {
        if (sunTransform != null)
        {
            Vector2 toSun = (sunTransform.position - transform.position).normalized;
            direction = Vector2.Lerp(direction.normalized, toSun, Time.deltaTime * 1.5f);
        }

        transform.position += (Vector3)(direction.normalized * speed * speedMultiplier * Time.deltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        if (transform.position.y < -6f)
        {
            GameManager.EnemyMissed();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Sun"))
        {
            Health sunHealth = other.GetComponent<Health>();
            if (sunHealth != null)
                sunHealth.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
