using System.Collections;
using UnityEngine;

/// <summary>
/// BulletSpawner — attach to any enemy/boss GameObject.
/// Supports: Radial, Aimed, Wave/Sine, and Spiral patterns.
/// Select your pattern in the Inspector dropdown.
/// </summary>
public class BulletSpawner : MonoBehaviour
{
    public enum BulletPattern { Radial, Aimed, Wave, Spiral, All }

    [Header("Pattern Selection")]
    public BulletPattern pattern = BulletPattern.Radial;  // Choose pattern in Inspector

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;       // Assign your bullet prefab here
    public float bulletSpeed = 8f;        // Base movement speed of bullets

    [Header("Radial Settings")]
    public int radialCount = 12;          // Number of bullets per radial burst
    public float radialFireRate = 0.5f;   // Seconds between bursts

    [Header("Aimed Settings")]
    public float aimedFireRate = 0.4f;    // Seconds between aimed shots
    public int aimedSpread = 3;           // Number of bullets in spread (odd = centred)
    public float aimedSpreadAngle = 15f;  // Degrees between each spread bullet

    [Header("Wave Settings")]
    public int waveCount = 10;            // Bullets per wave row
    public float waveFireRate = 0.08f;    // Seconds between each bullet in a wave
    public float waveAmplitude = 30f;     // Max angle offset for the sine wave
    public float waveFrequency = 2f;      // How fast the wave oscillates

    [Header("Spiral Settings")]
    public int spiralArms = 3;            // Number of spiral arms
    public float spiralFireRate = 0.05f;  // Seconds between each spiral step
    public float spiralAngleStep = 10f;   // Degrees to rotate per step

    // ── Internal ──────────────────────────────────────────────────────────────
    private Transform _player;
    private float _spiralAngle = 0f;
    private float _waveTime = 0f;

    void Start()
    {
        // Find player by tag — make sure your player GameObject is tagged "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            _player = playerObj.transform;

        StartSelectedPattern();
    }

    /// <summary>Starts only the coroutine(s) matching the selected pattern.</summary>
    void StartSelectedPattern()
    {
        StopAllCoroutines();

        switch (pattern)
        {
            case BulletPattern.Radial:
                StartCoroutine(FireRadial());
                break;
            case BulletPattern.Aimed:
                StartCoroutine(FireAimed());
                break;
            case BulletPattern.Wave:
                StartCoroutine(FireWave());
                break;
            case BulletPattern.Spiral:
                StartCoroutine(FireSpiral());
                break;
            case BulletPattern.All:
                StartCoroutine(FireRadial());
                StartCoroutine(FireAimed());
                StartCoroutine(FireWave());
                StartCoroutine(FireSpiral());
                break;
        }
    }

    /// <summary>
    /// Call this at runtime to switch patterns on the fly.
    /// e.g. SwitchPattern(BulletPattern.Spiral) from a boss phase script.
    /// </summary>
    public void SwitchPattern(BulletPattern newPattern)
    {
        pattern = newPattern;
        StartSelectedPattern();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>Spawn a bullet from this object's position at a given angle (degrees).</summary>
    void SpawnBullet(float angleDegrees)
    {
        if (bulletPrefab == null) return;

        float rad = angleDegrees * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }
        else
        {
            BulletMover mover = bullet.GetComponent<BulletMover>();
            if (mover != null)
                mover.SetDirection(direction, bulletSpeed);
        }

        Destroy(bullet, 6f);
    }

    /// <summary>Get the angle in degrees from this spawner towards the player.</summary>
    float AngleToPlayer()
    {
        if (_player == null) return 0f;
        Vector2 dir = (_player.position - transform.position).normalized;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    // ── Pattern 1: Radial ─────────────────────────────────────────────────────
    IEnumerator FireRadial()
    {
        while (true)
        {
            float angleStep = 360f / radialCount;
            for (int i = 0; i < radialCount; i++)
                SpawnBullet(i * angleStep);

            yield return new WaitForSeconds(radialFireRate);
        }
    }

    // ── Pattern 2: Aimed ──────────────────────────────────────────────────────
    IEnumerator FireAimed()
    {
        while (true)
        {
            if (_player != null)
            {
                float baseAngle = AngleToPlayer();
                int half = aimedSpread / 2;

                for (int i = -half; i <= half; i++)
                    SpawnBullet(baseAngle + i * aimedSpreadAngle);
            }
            yield return new WaitForSeconds(aimedFireRate);
        }
    }

    // ── Pattern 3: Wave / Sine ────────────────────────────────────────────────
    IEnumerator FireWave()
    {
        while (true)
        {
            for (int i = 0; i < waveCount; i++)
            {
                _waveTime += Time.deltaTime + waveFireRate;
                float sineOffset = Mathf.Sin(_waveTime * waveFrequency) * waveAmplitude;
                SpawnBullet(0f + sineOffset);
                yield return new WaitForSeconds(waveFireRate);
            }
        }
    }

    // ── Pattern 4: Spiral ─────────────────────────────────────────────────────
    IEnumerator FireSpiral()
    {
        while (true)
        {
            float armSpacing = 360f / spiralArms;
            for (int arm = 0; arm < spiralArms; arm++)
                SpawnBullet(_spiralAngle + arm * armSpacing);

            _spiralAngle += spiralAngleStep;
            if (_spiralAngle >= 360f) _spiralAngle -= 360f;

            yield return new WaitForSeconds(spiralFireRate);
        }
    }
}


/// <summary>
/// Simple bullet movement script — attach to your bullet prefab
/// as a fallback if it has no Rigidbody2D.
/// </summary>
public class BulletMover : MonoBehaviour
{
    private Vector2 _direction;
    private float _speed;

    public void SetDirection(Vector2 direction, float speed)
    {
        _direction = direction;
        _speed = speed;
    }

    void Update()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);
    }
}
