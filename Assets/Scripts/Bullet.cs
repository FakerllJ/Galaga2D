using UnityEngine;

// Serve para o tiro do jogador (direction 0,1) e dos inimigos (direction 0,-1)
public class Bullet : MonoBehaviour
{
    public Vector2 direction = Vector2.up;
    public float speed = 12f;

    void Update()
    {
        transform.Translate((Vector3)direction * speed * Time.deltaTime);

        Vector3 p = transform.position;
        if (Mathf.Abs(p.x) > 12f || Mathf.Abs(p.y) > 7f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Tiro inimigo atingindo o jogador
        if (CompareTag("EnemyBullet") && other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) player.TakeDamage();
            Destroy(gameObject);
        }
    }
}
