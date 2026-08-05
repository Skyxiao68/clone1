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
            enemy.Knockback(transform.position, 5f);
            Debug.Log("enemy knocked ");
        }
        
        //Add boss damage 
        BossEnemyScript boss = col.GetComponent<BossEnemyScript>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            
            
        }
    }
}
