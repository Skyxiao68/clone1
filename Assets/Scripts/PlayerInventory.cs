using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public GameObject bombPrefab;

    public Transform bombSpawnPoint;

    /* public void OnUseBomb(InputAction.CallbackContext context)
     {
         //Debug.Log("E pressed");
         if (!context.performed)
             return;
         Debug.Log("E pressed");

         //  console.log("I punched e");

         if (GameManager.Instance.UseBomb())
         {
             Instantiate(bombPrefab,
                         bombSpawnPoint.position,
                         Quaternion.identity);
         }
     }*/

    public void UseBomb(InputAction.CallbackContext context)
    {
        Debug.Log("E Pressed");

        if (!context.performed)
            return;

        if (GameManager.Instance.UseBomb())
        {
            Instantiate(bombPrefab,
                        bombSpawnPoint.position,
                        Quaternion.identity);
        }
    }
}
