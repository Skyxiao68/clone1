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

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)

    {
        if (!other.CompareTag("Player"))
            return;

        switch(type)
        {
            case CollectibleType.Triforce:
                GameManager.Instance.AddTriforce();
                break;

            case CollectibleType.Bomb:
                GameManager.Instance.AddBomb();
                break;

            case CollectibleType.Key:
                GameManager.Instance.AddKey();
                break;

            case CollectibleType.Chest:
                GameManager.Instance.AddChest();
                break;


        }

        if (audioSource != null)
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);

        //SGameManager.Instance.UpdateUI();
        Destroy(gameObject);
    }

}
