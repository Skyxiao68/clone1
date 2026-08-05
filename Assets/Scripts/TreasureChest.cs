using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    public enum RewardType { Attack, Speed }
    [SerializeField] private RewardType rewardType;
    [SerializeField] private float rewardAmount = 1f;
    

    private bool isOpened = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isOpened)
            return;

        PlayerStats stats = other.GetComponent<PlayerStats>();
        if (stats == null) return;

        switch (rewardType)
        {
            case RewardType.Attack:
                stats.IncreaseAttack(rewardAmount);
                break;
            case RewardType.Speed:
                stats.IncreaseSpeed(rewardAmount);
                break;
        }

        isOpened = true;


        Destroy(gameObject);
    }
}