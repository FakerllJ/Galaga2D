using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float speed = 8f;
    public float limitX = 8f;

    [Header("Tiro")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float doubleShotCooldown = 0.25f;

    [Header("Extras")]
    public GameObject shieldVisual;

    [Header("Sons")]
    public AudioClip shootSound;
    public AudioClip hitSound;

    GameObject currentBullet;
    SpriteRenderer sr;
    float doubleShotUntil;
    float shieldUntil;
    float invincibleUntil;
    float nextShotTime;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;   // jogo pausado ou acabou

        Move();
        if (Input.GetKeyDown(KeyCode.Space)) Shoot();
        UpdateVisuals();
    }

    void Move()
    {
        float h = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) h -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) h += 1f;

        transform.Translate(Vector3.right * h * speed * Time.deltaTime);

        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, -limitX, limitX);
        transform.position = p;
    }

    void Shoot()
    {
        if (bulletPrefab == null) return;
        Vector3 pos = firePoint != null ? firePoint.position : transform.position;

        if (Time.time < doubleShotUntil)            // power-up Tiro Duplo ativo
        {
            if (Time.time < nextShotTime) return;
            nextShotTime = Time.time + doubleShotCooldown;
            Instantiate(bulletPrefab, pos + Vector3.left * 0.3f, Quaternion.identity);
            Instantiate(bulletPrefab, pos + Vector3.right * 0.3f, Quaternion.identity);
        }
        else                                        // normal: so 1 tiro por vez
        {
            if (currentBullet != null) return;
            currentBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
        }
        Sfx.Play(shootSound);
    }

    public void ActivateDoubleShot(float seconds)
    {
        doubleShotUntil = Time.time + seconds;
    }

    public void ActivateShield(float seconds)
    {
        shieldUntil = Time.time + seconds;
    }

    public void TakeDamage()
    {
        if (Time.time < invincibleUntil) return;

        if (Time.time < shieldUntil)                // escudo absorve 1 ataque
        {
            shieldUntil = 0f;
            invincibleUntil = Time.time + 0.5f;
            return;
        }

        Sfx.Play(hitSound);
        invincibleUntil = Time.time + 2f;           // 2s piscando, sem levar dano
        GameManager.Instance.LoseLife();
    }

    void UpdateVisuals()
    {
        if (shieldVisual != null) shieldVisual.SetActive(Time.time < shieldUntil);

        if (sr != null)
        {
            bool blinkOff = Time.time < invincibleUntil
                            && Mathf.FloorToInt(Time.time * 10f) % 2 == 1;
            sr.enabled = !blinkOff;
        }
    }
}
