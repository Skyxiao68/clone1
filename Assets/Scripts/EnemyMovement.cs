 using System;
 using System.Collections;
 
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D enemy;
    private Vector2 moveInput;
    private Animator animator;
    private bool chase;

    public float movementSpeed = 2f;
    public float chaseDistance = 5f; //only start chasing when a certain distance away
    public Transform target;

    public int enemyHealth ;
    public GameObject damageSquare;

    public int maxHealth = 3;
    
    //Enemy Attack System
    public float attackedCooldown = 2f;
    public bool canAttack = true;
    

    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyHealth = maxHealth;
        damageSquare.SetActive(false);
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        float distance = Vector2.Distance(enemy.position, target.position);
        //Calculate the distance away from player and enemy 

        chase = distance <= chaseDistance;

        if (chase)
        {
            moveInput = ((Vector2)target.position - enemy.position).normalized;

            animator.SetBool("isWalking", true);

            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        else
        {
            moveInput = Vector2.zero;
            animator.SetBool("isWalking", false);
        }

       
    }

    void FixedUpdate()
    {
        enemy.linearVelocity = moveInput * movementSpeed;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canAttack)
        {
            StartCoroutine(AttackPlayer(collision.gameObject));
        }
    }
    
    IEnumerator AttackPlayer(GameObject player)
    {
        canAttack = false;

        // Play fake attack animation
        StartCoroutine(AttackAnimation());

        // Wait until the sword actually swings
        yield return new WaitForSeconds(1f);

        PlayerMovement playerScript = player.GetComponent<PlayerMovement>();

       playerScript.TakeDamage(1);

        // Wait before another attack
        yield return new WaitForSeconds(attackedCooldown);

        canAttack = true;
    }

    IEnumerator AttackAnimation()
    {
         Vector3 originalScale = transform.localScale;
        
            transform.localScale = originalScale * 1.5f;
        
            yield return new WaitForSeconds(0.2f);
        
            transform.localScale = originalScale;
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        StartCoroutine(ShowDamageSquare());
        //can set damage in collision 

        if (enemyHealth <=0)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator ShowDamageSquare()
    {
        damageSquare.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        damageSquare.SetActive(false);
    }
    
    
}