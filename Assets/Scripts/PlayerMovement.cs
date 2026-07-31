using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 4f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    // Health System
    public int lives = 3;
    public GameObject[] heartSprites;

    // Melee Attack System
    public Transform aim;
    
    public GameObject melee;
    private bool isAttacking = false;
    public float attackDuration = 0.3f;
    public float attackTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Rotate the aim object to one of 8 directions
        if (moveInput != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

            // Snap to nearest 45 degrees
            angle = Mathf.Round(angle / 45f) * 45f;

            // If your sword points UP by default, change this to angle + 90
            aim.rotation = Quaternion.Euler(0f, 0f, angle+90f);
        }
        CheckMeleeTimer();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * movementSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        bool isWalking = moveInput != Vector2.zero;
        animator.SetBool("isWalking", isWalking);

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        if (isWalking)
        {
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }
    
    public void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack test");
        if (!context.performed)
            return;

        
        OnAttack();
    }

    void OnAttack()
    {
        if (!isAttacking)
        {
            melee.SetActive(true);
            isAttacking = true;
            //Call animator to play melee attack here 
            
            
        }
    }

    void CheckMeleeTimer()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDuration)
            {
                attackTimer = 0;
                isAttacking = false;
                melee.SetActive(false);
            }
        }
    }

    public void RestoreLives()
    {
        if (lives < 3)
        {
            lives = 3;

            foreach (GameObject heart in heartSprites)
            {
                heart.SetActive(true);
            }
        }
    }
}