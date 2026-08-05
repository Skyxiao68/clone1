using System;
using System.Collections;
using UnityEngine;

public class BossEnemyScript : MonoBehaviour
{
    private Rigidbody2D enemy;
    private Vector2 moveInput;
    private Animator animator;
    private bool chase;

    [Header("Movement")]
    public float movementSpeed = 2f;
    public float chaseDistance = 6f;
    public Transform target;

    [Header("Health")]
    public int maxHealth = 30;
    public int enemyHealth;
    public GameObject damageSquare;

    [Header("Boss Attack")]
    public float attackedCooldown = 10f;
    public bool canAttack = true;

    public Transform axePivot;
    public float spinSpeed = 360f;
    public float attackDuration = 5f;

    public GameObject winCanvas; 
    
   

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
            return;

        float distance = Vector2.Distance(enemy.position, target.position);

        // Calculate distance between boss and player
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

        // Play spin attack for boss
        StartCoroutine(SpinAttack());

        // Wait until the attack connects
        yield return new WaitForSeconds(1f);

        // PlayerMovement playerScript = player.GetComponent<PlayerMovement>();
        //
        // playerScript.TakeDamage(1);

        // Wait before another attack
        yield return new WaitForSeconds(attackedCooldown);

        canAttack = true;
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;

        StartCoroutine(ShowDamageSquare());

        if (enemyHealth <= 0)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
            Die(); 
        }
    }

    
    public void Die()
    {
        Time.timeScale = 0f; 
      
        this.enabled = false;
    
    
        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
        }
    
    }
    

    IEnumerator ShowDamageSquare()
    {
        damageSquare.SetActive(true);

        yield return new WaitForSeconds(0.15f);

        damageSquare.SetActive(false);
    }

    IEnumerator SpinAttack()
    {
        float timer = 0f;

        while (timer < attackDuration)
        {
            axePivot.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

            timer += Time.deltaTime;

            yield return null;
        }
    }
}