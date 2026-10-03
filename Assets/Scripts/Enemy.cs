using UnityEngine;

public enum EnemyType { A, B, Elite }

public class Enemy : MonoBehaviour
{
    public EnemyType type = EnemyType.A;
    public int health = 1;
    public int points = 100;
    public float speed = 1.5f;

    [Header("Zigue-zague (tipo B)")]
    public float zigzagSpeed = 3f;
    public float zigzagWidth = 2f;

    [Header("Tiro (0 = nao atira)")]
    public float shootInterval = 0f;
    public GameObject bulletPrefab;

    [Header("Som")]
    public AudioClip explosionSound;

    float startX;
    float birthTime;
    float nextShootTime;
    bool dead;

    void Start()
    {
        startX = transform.position.x;
        birthTime = Time.time;
        nextShootTime = Time.time + Random.Range(1f, 2.5f);
    }

    void Update()
    {
        Vector3 p = transform.position;
        p.y -= speed * Time.deltaTime;

        if (type == EnemyType.B)
            p.x = startX + Mathf.Sin((Time.time - birthTime) * zigzagSpeed) * zigzagWidth;

        transform.position = p;

        if (shootInterval > 0f && bulletPrefab != null && Time.time >= nextShootTime)
        {
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            nextShootTime = Time.time + shootInterval;
        }

        if (p.y < -6f) Leave();                     // passou da nave e saiu da tela
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            Destroy(other.gameObject);
            TakeDamage(1);
        }
        else if (other.CompareTag("Player"))        // bateu na nave
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) player.TakeDamage();
            Die(false);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Die(true);
    }

    void Die(bool byPlayer)
    {
        if (dead) return;
        dead = true;

        if (byPlayer)
        {
            GameManager.Instance.AddScore(points);
            EnemySpawner.Instance.TryDropPowerUp(transform.position);
        }
        Sfx.Play(explosionSound);
        Finish();
    }

    void Leave()
    {
        if (dead) return;
        dead = true;
        Finish();
    }

    void Finish()
    {
        EnemySpawner.Instance.EnemyGone();
        Destroy(gameObject);
    }
}
