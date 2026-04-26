using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameObject spark = new GameObject("Spark");
            spark.transform.position = other.transform.position;
            SpriteRenderer sr = spark.AddComponent<SpriteRenderer>();
            sr.sprite = GetComponent<SpriteRenderer>().sprite;
            sr.color = new Color(1f, 0.5f, 0f);
            spark.transform.localScale = new Vector3(1f, 1f, 1);
            Destroy(spark, 0.15f);

            Destroy(other.gameObject);
            Destroy(gameObject);
            GameManager.AddScore();
        }
    }
}