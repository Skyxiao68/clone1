using UnityEngine;

public class KeyLockInteraction : MonoBehaviour
{
    
    private bool isDestroyed = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isDestroyed)
            {
                return;
            }

        
        if (!GameManager.Instance.UseKey())
           {
             return;
           }

        isDestroyed = true;

      

        GameManager.Instance.OnKeyLockDestroyed();
        Destroy(gameObject);
    }
}