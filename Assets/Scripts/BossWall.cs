using UnityEngine;

public class BossWall : MonoBehaviour
{
    [Header("NPCs Required to Vanish the Wall")]
    [SerializeField] private GameObject npc1;
    [SerializeField] private GameObject npc2;
    [SerializeField] private GameObject npc3;

    [SerializeField] private bool destroyOnVanish = true;

    private bool npc1Collided = false;
    private bool npc2Collided = false;
    private bool npc3Collided = false;
    private bool hasVanished = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasVanished) return;

        if (other.gameObject == npc1) npc1Collided = true;
        else if (other.gameObject == npc2) npc2Collided = true;
        else if (other.gameObject == npc3) npc3Collided = true;
        else return;

        if (npc1Collided && npc2Collided && npc3Collided)
        {
            VanishWall();
        }
    }

    private void VanishWall()
    {
        hasVanished = true;

        if (destroyOnVanish)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}