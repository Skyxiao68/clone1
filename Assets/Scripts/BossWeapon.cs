using System;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerMovement player = col.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.TakeDamage(damage);
                player.Knockback(transform.position,20f);
                Debug.Log("Boss weapon hit ");
                
            }
        }
    }
}
