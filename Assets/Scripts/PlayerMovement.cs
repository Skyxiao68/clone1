using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // add this

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 4f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;

    //Health System
    public int lives = 3;
    public GameObject[] heartSprites;

    public Button reflectBtn;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        rb.linearVelocity = moveInput * movementSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking",true);
        if (context.canceled)
        {
            animator.SetBool("isWalking",false);
            animator.SetFloat("LastInputX",moveInput.x);
            animator.SetFloat("LastInputY",moveInput.y);
        }
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX",moveInput.x);
        animator.SetFloat("InputY",moveInput.y);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Attack"))
        {
            if (lives > 0)
            {
                lives--;
                heartSprites[lives].SetActive(false);
            }

            if (lives == 0)
            {
                Death();
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

    public void Death()
    {
        if (lives == 0)
        {
            SceneManager.LoadScene("DeathScreen");
        }
    }
}