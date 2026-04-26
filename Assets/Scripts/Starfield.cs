using UnityEngine;

public class Starfield : MonoBehaviour
{
    public GameObject starPrefab;
    public int starCount = 80;
    public float spreadX = 10f;
    public float spreadY = 7f;

    void Start()
    {
        for (int i = 0; i < starCount; i++)
        {
            float x = Random.Range(-spreadX, spreadX);
            float y = Random.Range(-spreadY, spreadY);
            GameObject star = Instantiate(starPrefab, new Vector3(x, y, 0), Quaternion.identity);
            float scale = Random.Range(0.05f, 0.15f);
            star.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}