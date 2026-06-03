using UnityEngine;
using System.Collections;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public Path path;
    public Wave[] waves;
    private int currentWave = 0;

    [SerializeField]
    private int debugEnemiesAlive;

    [Header("UI")]
    public TextMeshProUGUI waveMessage;
    public float waveMessageDuration = 3f;

    void Start()
    {
        ContadorEnem.RecalculateFromScene();
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < waves.Length)
        {
            Wave wave = waves[currentWave];

            yield return new WaitForSeconds(wave.startDelay);

            if (waveMessage != null)
            {
                StartCoroutine(ShowWaveStart(currentWave + 1, waveMessageDuration));
                yield return new WaitForSeconds(waveMessageDuration);
            }

            int[] remaining = new int[wave.enemies.Length];

            for (int i = 0; i < wave.enemies.Length; i++)
            {
                remaining[i] = wave.enemies[i].amount;
            }

            int totalEnemies = 0;

            foreach (Enemy enemy in wave.enemies)
            {
                totalEnemies += enemy.amount;
            }

            int spawnedEnemies = 0;
            int initialEnemyCount = totalEnemies;

            while (totalEnemies > 0)
            {
                System.Collections.Generic.List<int> availableTypes =
                    new System.Collections.Generic.List<int>();

                for (int i = 0; i < wave.enemies.Length; i++)
                {
                    bool enemigosDisponibles = remaining[i] > 0;
                    bool desbloqueo = spawnedEnemies >= wave.enemies[i].unlockAfter;

                    if (enemigosDisponibles && desbloqueo)
                    {
                        availableTypes.Add(i);
                    }
                }

                if (availableTypes.Count == 0)
                {
                    yield return null;
                    continue;
                }

                int randomIndex =
                    availableTypes[Random.Range(0, availableTypes.Count)];

                Enemy selectedEnemy = wave.enemies[randomIndex];

                SpawnEnemy(selectedEnemy.enemyPrefab);

                remaining[randomIndex]--;
                totalEnemies--;
                spawnedEnemies++;

                float currentRate = selectedEnemy.spawnRate;

                if (totalEnemies <= initialEnemyCount / 2)
                {
                    currentRate *= 0.5f;
                }

                if (totalEnemies == 0)
                {
                    currentRate /= 0.5f;
                }

                yield return new WaitForSeconds(currentRate);
            }

            ContadorEnem.RecalculateFromScene();
            debugEnemiesAlive = ContadorEnem.Alive;

            while (ContadorEnem.Alive > 0)
            {
                yield return null;
            }

            if (waveMessage != null)
            {
                StartCoroutine(ShowWaveComplete(currentWave + 1, waveMessageDuration));
            }

            yield return new WaitForSeconds(wave.timeBetweenWaves);

            currentWave++;
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(
            enemyPrefab,
            transform.position,
            Quaternion.identity
        );

        enemy.GetComponent<EnemyAI2D>()
            .SetPath(path.waypoints);

        ContadorEnem.Increment();
        debugEnemiesAlive = ContadorEnem.Alive;
    }

    private IEnumerator ShowWaveStart(int waveNumber, float duration)
    {
        if (waveMessage == null) yield break;

        waveMessage.gameObject.SetActive(true);
        waveMessage.text = $"Inicio ronda {waveNumber}";

        yield return new WaitForSeconds(duration);

        waveMessage.gameObject.SetActive(false);
    }

    private IEnumerator ShowWaveComplete(int waveNumber, float duration)
    {
        if (waveMessage == null) yield break;

        waveMessage.gameObject.SetActive(true);
        waveMessage.text = $"Ronda {waveNumber} completada";

        yield return new WaitForSeconds(duration);

        waveMessage.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int amount;
    public float spawnRate;

    [Header("Desbloqueo")]
    public int unlockAfter;
}

[System.Serializable]
public class Wave
{
    public Enemy[] enemies;
    public float timeBetweenWaves;
    public float startDelay;
}