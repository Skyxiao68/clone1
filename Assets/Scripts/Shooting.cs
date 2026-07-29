using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Shooting : MonoBehaviour
{
    public enum GunType { Pistol, Shotgun, AssaultRifle }

    [System.Serializable]
    private struct GunSettings
    {
        public string name;
        public float fireRate;
        public float range;
        public int pelletCount;
        public float spreadAngle;
        public int magazineSize;
    }

    [Header("References")]
    public Transform ShootPos;
    public GameObject bulletPrefab;

    [Header("Projectile Settings")]
    [SerializeField] private float bulletSpeed = 20f;

    [Header("Aiming")]
    [SerializeField] private Camera cam;

    [Header("Gun Configs")]
    [SerializeField]
    private GunSettings pistolSettings = new GunSettings
    {
        name = "Pistol",
        fireRate = 0.4f,
        range = 15f,
        pelletCount = 1,
        spreadAngle = 0f,
        magazineSize = 3
    };

    [SerializeField]
    private GunSettings shotgunSettings = new GunSettings
    {
        name = "Shotgun",
        fireRate = 0.8f,
        range = 8f,
        pelletCount = 3,
        spreadAngle = 45f,
        magazineSize = 1
    };

    [SerializeField]
    private GunSettings assaultRifleSettings = new GunSettings
    {
        name = "AssaultRifle",
        fireRate = 0.15f,
        range = 8f,
        pelletCount = 1,
        spreadAngle = 0f,
        magazineSize = 10
    };

    [Header("Reload Settings")]
    [SerializeField] private float reloadTime = 1f;
    [SerializeField] private AudioSource pistolReloadAudio;
    [SerializeField] private AudioSource shotgunReloadAudio;
    [SerializeField] private AudioSource assaultRifleReloadAudio;

    [Header("Shoot Audio")]
    [SerializeField] private AudioSource pistolShootAudio;
    [SerializeField] private AudioSource shotgunShootAudio;
    [SerializeField] private AudioSource assaultRifleShootAudio;

    private GunType currentGun = GunType.Pistol;
    private float lastFireTime = -999f;
    private int currentAmmo;
    private bool isReloading = false;

    private bool pistolUnlocked = true;
    private bool shotgunUnlocked = false;
    private bool assaultRifleUnlocked = false;

    private PlayerInput playerInput;
    private InputAction fireAction;
    private InputAction pistolAction;
    private InputAction shotgunAction;
    private InputAction assaultRifleAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (cam == null) cam = Camera.main;
    }

    private void Start()
    {
        currentAmmo = GetCurrentSettings().magazineSize;
    }

    private void OnEnable()
    {
        var actions = playerInput.actions;

        fireAction = actions.FindAction("Fire");
        pistolAction = actions.FindAction("Pistol");
        shotgunAction = actions.FindAction("Shotgun");
        assaultRifleAction = actions.FindAction("AssaultRifle");

        if (fireAction == null)
        {
            Debug.LogError("Could not find an action called 'Fire'.", this);
        }
        else
        {
            fireAction.performed += OnFirePerformed;
            fireAction.Enable();
        }

        SetupSwitchAction(pistolAction, "Pistol", () => SwitchGun(GunType.Pistol));
        SetupSwitchAction(shotgunAction, "Shotgun", () => SwitchGun(GunType.Shotgun));
        SetupSwitchAction(assaultRifleAction, "AssaultRifle", () => SwitchGun(GunType.AssaultRifle));
    }

    private void SetupSwitchAction(InputAction action, string actionName, System.Action onPerformed)
    {
        if (action == null)
        {
            Debug.LogWarning($"Could not find an action called '{actionName}'.", this);
            return;
        }

        action.performed += ctx => onPerformed();
        action.Enable();
    }

    private void OnDisable()
    {
        if (fireAction != null) fireAction.performed -= OnFirePerformed;
    }

    private void Update()
    {
        AimAtMouse();
        HandleAutoFire();
    }

    private void AimAtMouse()
    {
        if (Mouse.current == null || cam == null || ShootPos == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = cam.ScreenToWorldPoint(
            new Vector3(mouseScreenPos.x, mouseScreenPos.y, -cam.transform.position.z));

        Vector2 direction = (Vector2)mouseWorldPos - (Vector2)ShootPos.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        ShootPos.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void HandleAutoFire()
    {
        if (currentGun != GunType.AssaultRifle) return;
        if (isReloading) return;
        if (fireAction == null || !fireAction.IsPressed()) return;

        GunSettings settings = GetCurrentSettings();

        if (Time.time - lastFireTime < settings.fireRate) return;
        lastFireTime = Time.time;

        TryFire(settings);
    }

    private bool IsGunUnlocked(GunType gun)
    {
        switch (gun)
        {
            case GunType.Shotgun: return shotgunUnlocked;
            case GunType.AssaultRifle: return assaultRifleUnlocked;
            default: return pistolUnlocked;
        }
    }

    private void SwitchGun(GunType newGun)
    {
        if (isReloading) return;
        if (!IsGunUnlocked(newGun)) return;

        currentGun = newGun;
        currentAmmo = GetCurrentSettings().magazineSize;
    }

    /// <summary>
    /// Called by a GunPickup in the world when the player collides with it.
    /// </summary>
    public void UnlockGun(GunType gun)
    {
        switch (gun)
        {
            case GunType.Shotgun:
                shotgunUnlocked = true;
                break;
            case GunType.AssaultRifle:
                assaultRifleUnlocked = true;
                break;
            default:
                pistolUnlocked = true;
                break;
        }

        SwitchGun(gun);
    }

    private GunSettings GetCurrentSettings()
    {
        switch (currentGun)
        {
            case GunType.Shotgun: return shotgunSettings;
            case GunType.AssaultRifle: return assaultRifleSettings;
            default: return pistolSettings;
        }
    }

    private AudioSource GetCurrentReloadAudio()
    {
        switch (currentGun)
        {
            case GunType.Shotgun: return shotgunReloadAudio;
            case GunType.AssaultRifle: return assaultRifleReloadAudio;
            default: return pistolReloadAudio;
        }
    }

    private AudioSource GetCurrentShootAudio()
    {
        switch (currentGun)
        {
            case GunType.Shotgun: return shotgunShootAudio;
            case GunType.AssaultRifle: return assaultRifleShootAudio;
            default: return pistolShootAudio;
        }
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        if (currentGun == GunType.AssaultRifle) return;
        if (isReloading) return;

        GunSettings settings = GetCurrentSettings();

        if (Time.time - lastFireTime < settings.fireRate) return;
        lastFireTime = Time.time;

        TryFire(settings);
    }

    private void TryFire(GunSettings settings)
    {
        if (currentAmmo <= 0) return;

        FireGun(settings);
        PlayShootAudio();
        currentAmmo--;

        if (currentAmmo <= 0)
        {
            StartCoroutine(ReloadRoutine(settings));
        }
    }

    private void PlayShootAudio()
    {
        AudioSource shootAudio = GetCurrentShootAudio();
        if (shootAudio != null)
        {
            shootAudio.Play();
        }
    }

    private IEnumerator ReloadRoutine(GunSettings settings)
    {
        isReloading = true;

        AudioSource reloadAudio = GetCurrentReloadAudio();
        if (reloadAudio != null)
        {
            reloadAudio.Play();
        }

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = settings.magazineSize;
        isReloading = false;
    }

    private void FireGun(GunSettings settings)
    {
        if (bulletPrefab == null || ShootPos == null)
        {
            Debug.LogWarning("Missing bulletPrefab or ShootPos assignment on the Shooting script!", this);
            return;
        }

        int count = Mathf.Max(1, settings.pelletCount);

        float startAngle = count > 1 ? -settings.spreadAngle / 2f : 0f;
        float angleStep = count > 1 ? settings.spreadAngle / (count - 1) : 0f;

        for (int i = 0; i < count; i++)
        {
            float offset = startAngle + angleStep * i;
            SpawnBullet(offset, settings.range);
        }
    }

    private void SpawnBullet(float angleOffsetDegrees, float range)
    {
        Quaternion rotation = ShootPos.rotation * Quaternion.Euler(0f, 0f, angleOffsetDegrees);
        GameObject newBullet = Instantiate(bulletPrefab, ShootPos.position, rotation);

        Rigidbody2D rb2d = newBullet.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.linearVelocity = (Vector2)(rotation * Vector3.right) * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet prefab has no Rigidbody2D — it won't move.", newBullet);
        }

        float lifetime = range / bulletSpeed;
        Destroy(newBullet, lifetime);
    }
}