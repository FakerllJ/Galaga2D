using UnityEngine;

public enum PowerUpType { DoubleShot, Shield, ExtraLife }

public class PowerUp : MonoBehaviour
{
    public PowerUpType type;
    public float fallSpeed = 2f;
    public AudioClip pickupSound;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
        if (transform.position.y < -6f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        switch (type)
        {
            case PowerUpType.DoubleShot: player.ActivateDoubleShot(15f); break;
            case PowerUpType.Shield:     player.ActivateShield(10f);     break;
            case PowerUpType.ExtraLife:  GameManager.Instance.AddLife(); break;
        }

        GameManager.Instance.AddScore(50);
        Sfx.Play(pickupSound);
        Destroy(gameObject);
    }
}
