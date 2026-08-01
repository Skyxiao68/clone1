using System;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        EnemyMovement enemy = col.GetComponent<EnemyMovement>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
