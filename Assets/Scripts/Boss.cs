using UnityEngine;

public class Boss : MonoBehaviour
{
    public int health = 40;
    public float speed = 3f;
    public float limitX = 6f;
    public float shootInterval = 1.2f;
    public GameObject bulletPrefab;
    public AudioClip explosionSound;

    int direction = 1;
    float nextShootTime;
    bool dead;

    void Update()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);
        if (transform.position.x > limitX) direction = -1;
        if (transform.position.x < -limitX) direction = 1;

        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootInterval;
            Fire();
        }
    }

    void Fire()                                     // leque de 5 tiros
    {
        for (int i = -2; i <= 2; i++)
        {
            GameObject b = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            b.GetComponent<Bullet>().direction = new Vector2(i * 0.3f, -1f).normalized;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (dead || !other.CompareTag("PlayerBullet")) return;

        Destroy(other.gameObject);
        health--;
        if (health > 0) return;

        dead = true;
        GameManager.Instance.AddScore(5000);
        Sfx.Play(explosionSound);
        Destroy(gameObject);
        GameManager.Instance.Victory();
    }
}
