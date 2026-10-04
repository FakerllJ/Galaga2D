using UnityEngine;

// Fundo que rola durante as fases e para na fase do Boss.
// Use este script SO no Fundo_A e arraste o Fundo_B para o campo "Copy B".
public class BackgroundScroller : MonoBehaviour
{
    public float speed = 1f;
    public Transform copyB;                 // arraste o Fundo_B aqui
    public float bossSpeedMultiplier = 3f;  // acelera ate centralizar e parar

    float height;
    bool stopped;

    void Start()
    {
        Fit(GetComponent<SpriteRenderer>());
        height = Fit(copyB.GetComponent<SpriteRenderer>());
        SetY(transform, 0f);
        SetY(copyB, height); // fica logo acima da primeira
    }

    void Update()
    {
        if (stopped) return;

        bool boss = GameManager.Instance != null && GameManager.Instance.level >= 4;
        float move = speed * (boss ? bossSpeedMultiplier : 1f) * Time.deltaTime;

        Vector3 a = transform.position;
        Vector3 b = copyB.position;
        float prevA = a.y;
        float prevB = b.y;
        a.y -= move;
        b.y -= move;

        // loop: quando uma copia sai por baixo, vai para cima da outra
        if (a.y <= -height) a.y += height * 2f;
        if (b.y <= -height) b.y += height * 2f;

        // fase do Boss: para quando uma copia chega ao centro da tela
        if (boss)
        {
            if (prevA > 0f && a.y <= 0f) { a.y = 0f; b.y = height; stopped = true; }
            else if (prevB > 0f && b.y <= 0f) { b.y = 0f; a.y = height; stopped = true; }
        }

        transform.position = a;
        copyB.position = b;
    }

    // Escala a imagem para cobrir a tela inteira e devolve a altura final
    float Fit(SpriteRenderer r)
    {
        Camera cam = Camera.main;
        float camH = cam.orthographicSize * 2f;
        float camW = camH * cam.aspect;
        Vector2 size = r.sprite.rect.size / r.sprite.pixelsPerUnit;
        float s = Mathf.Max(camW / size.x, camH / size.y) * 1.1f; // 10% de folga
        r.transform.localScale = new Vector3(s, s, 1f);
        return size.y * s;
    }

    void SetY(Transform t, float y)
    {
        t.position = new Vector3(t.position.x, y, t.position.z);
    }
}