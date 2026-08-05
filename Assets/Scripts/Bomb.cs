using UnityEngine;

public class Bomb : MonoBehaviour
{
  
    public float timer = 2f;

    public GameObject explosionEffect;

    public float explosionRadius = 2f;

    public int damage = 3;

    void Start()
    {
        Invoke(nameof(Explode), timer);
    }

    void Explode()
    {
        // Spawn explosion animation
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Find every collider in the radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            EnemyMovement enemy = hit.GetComponent<EnemyMovement>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Attack"))
        {
            EnemyMovement enemy = other.GetComponent<EnemyMovement>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            // Destroy the bomb after damaging the enemy
            Destroy(gameObject);
        }
    }
}
