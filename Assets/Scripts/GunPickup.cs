using UnityEngine;
using TMPro;
using System.Collections;

public class GunPickup : MonoBehaviour
{
    [SerializeField] private Shooting.GunType gunToUnlock;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool destroyOnPickup = true;
    [SerializeField] public TextMeshProUGUI textBox;
    [SerializeField] private float messageDuration = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        Shooting shooting = other.GetComponent<Shooting>();
        if (shooting == null) return;

        shooting.UnlockGun(gunToUnlock);

        if (textBox != null)
        {
            textBox.text = $"{gunToUnlock} unlocked!: Use numpad to switch Guns -> 1 = Pistol, 2 = Shotgun, 3 = Assault Gun";
            textBox.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay());
        }

        if (destroyOnPickup)
        {
            Destroy(gameObject,3f);
        }
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        //  if (textBox != null)
        //{
        textBox.text = string.Empty;
        // }
    }
}

