using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    public GameObject bombPrefab;

    public Transform bombSpawnPoint;

    public void OnUseBomb(InputAction.CallbackContext context)
    {
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
