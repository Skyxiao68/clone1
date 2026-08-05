using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public void IncreaseAttack(float amount)
    {
        
        Debug.Log(" incease attack" + amount);
    }

    public void IncreaseSpeed(float amount)
    {
        if (playerMovement != null)
        {
            playerMovement.movementSpeed += amount;
            Debug.Log("increase movement speed：" + playerMovement.movementSpeed);
        }
    }
}