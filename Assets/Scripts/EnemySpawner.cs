using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    [Header("Prefabs")]
    public GameObject enemyA;
    public GameObject enemyB;
    public GameObject enemyElite;
    public GameObject bossPrefab;
    public GameObject[] powerUps;

    [Header("Area de spawn")]
    public float spawnY = 6f;
    public float limitX = 6f;

    int alive;      // inimigos vivos na tela

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(LevelRoutine(GameManager.Instance.level));
    }

    IEnumerator LevelRoutine(int level)
    {
        yield return new WaitForSeconds(2f);

        if (level >= 4)                             // fase do Boss
        {
            Instantiate(bossPrefab, new Vector3(0f, 3.5f, 0f), Quaternion.identity);
            yield break;
        }

        int count = 8 + level * 4;                  // fase 1: 12, fase 2: 16, fase 3: 20
        float interval = 1.4f - level * 0.2f;       // fase 1: 1.2s, fase 2: 1.0s, fase 3: 0.8s

        for (int i = 0; i < count; i++)
        {
            SpawnEnemy(ChooseEnemy(level), level);
            yield return new WaitForSeconds(interval);
        }

        yield return new WaitUntil(() => alive <= 0);   // espera limpar a tela

        GameManager.Instance.NextLevel();
        StartCoroutine(LevelRoutine(GameManager.Instance.level));
    }

    GameObject ChooseEnemy(int level)
    {
        float r = Random.value;
        if (level == 1) return enemyA;
        if (level == 2) return r < 0.6f ? enemyA : enemyB;
        return r < 0.4f ? enemyA : (r < 0.75f ? enemyB : enemyElite);
    }

    void SpawnEnemy(GameObject prefab, int level)
    {
        Vector3 pos = new Vector3(Random.Range(-limitX, limitX), spawnY, 0f);
        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        go.GetComponent<Enemy>().speed *= 1f + (level - 1) * 0.35f;   // fica mais rapido
        alive++;
    }

    public void EnemyGone()
    {
        alive--;
    }

    public void TryDropPowerUp(Vector3 pos)
    {
        if (powerUps == null || powerUps.Length == 0) return;

        float chance = GameManager.Instance.level >= 3 ? 0.25f : 0.12f;
        if (Random.value < chance)
            Instantiate(powerUps[Random.Range(0, powerUps.Length)], pos, Quaternion.identity);
    }
}
