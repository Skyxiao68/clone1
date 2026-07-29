using System;
using Unity.VisualScripting;
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

    public int enemyHealth = 5;
    //public GameObject bullet;
    

    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        //can set damage in collision 

        if (enemyHealth < 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
            col.gameObject.SetActive(false);
            Debug.Log("Collided ");
        }
        
    }
    
    
}