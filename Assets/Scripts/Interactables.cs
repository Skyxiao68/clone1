using UnityEngine;
using TMPro;

public class Interactables : MonoBehaviour
{

    public enum CollectibleType
    {
        Triforce,
        Bomb,
        Key,
        Chest
    }

    public CollectibleType type;

    private void OnTriggerEnter2D(Collider2D other)

    {
        if (!other.CompareTag("Player"))
            return;

        switch(type)
        {
            case CollectibleType.Triforce:
                GameManager.Instance.triforce++;
                break;

            case CollectibleType.Bomb:
                GameManager.Instance.bombs++;
                break;

            case CollectibleType.Key:
                GameManager.Instance.keys++;
                break;

            case CollectibleType.Chest:
                GameManager.Instance.chests++;
                break;


        }

        GameManager.Instance.UpdateUI();
        Destroy(gameObject);
    }

}
