using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float timer = 2f;
    public GameObject explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke(nameof(Explode), timer);

    }


    void Explode()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
