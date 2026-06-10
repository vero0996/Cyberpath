using UnityEngine;
using System.Collections;
using System.Collections.Generic;   
using TMPro;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public Path path;
    public Wave[] waves;
    public int currentWave = 0;

    [SerializeField]
    private int debugEnemiesAlive;

    [Header("UI")]
    public TextMeshProUGUI waveMessage;
    public float waveMessageDuration = 3f;

    public static int WavesCompletadas;

    void Start()
    {
        // Leer wave guardada si existe
        int savedWave = PlayerPrefs.GetInt("Save_Wave", 0);
        if (savedWave > 0 && PlayerData.IsPaused)
        {
            currentWave = savedWave;
            Debug.Log($"EnemySpawner: Iniciando desde onda guardada {currentWave}");
        }
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
                List<int> availableTypes = new List<int>();

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

            PlayerData.SetWavesCompletadas(currentWave + 1);


            if (waveMessage != null)
            {
                StartCoroutine(ShowWaveComplete(currentWave + 1, waveMessageDuration));
            }

            if( currentWave == waves.Length - 1)
            {
                StartCoroutine(ShowVictory(waveMessageDuration));
                yield return new WaitForSeconds(waveMessageDuration);
                GuardarJuego guardarJuego = FindObjectOfType<GuardarJuego>();
                guardarJuego.ClearSavedData();  
                SceneManager.LoadScene("VICTORY");
            }
            yield return new WaitForSeconds(wave.timeBetweenWaves);

            if (currentWave > 0 && currentWave % 2 == 0) {
                foreach (Enemy enemy in wave.enemies)
                {
                    EnemyAI2D ai = enemy.enemyPrefab.GetComponent<EnemyAI2D>();
                    Damage damage = enemy.enemyPrefab.GetComponent<Damage>();

                    ai.speed *= 1.1f;
                    damage.hitPoints += 2;
                    damage.Dano += 1;
                }
            }
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
        waveMessage.text = $"ROUND {waveNumber} START";

        yield return new WaitForSeconds(duration);

        waveMessage.gameObject.SetActive(false);
    }

    private IEnumerator ShowWaveComplete(int waveNumber, float duration)
    {
        if (waveMessage == null) yield break;

        waveMessage.gameObject.SetActive(true);
        waveMessage.text = $"ROUND {waveNumber} COMPLETE";

        yield return new WaitForSeconds(duration);

        waveMessage.gameObject.SetActive(false);
    }
    private IEnumerator ShowVictory(float duration)
    {
        if (waveMessage == null) yield break;

        waveMessage.gameObject.SetActive(true);
        waveMessage.text = $"!!VICTORY!!";

        yield return new WaitForSeconds(duration);

        waveMessage.gameObject.SetActive(false);
    }
    // Obtener n�mero inicial de enemigos de una wave
    public int GetInitialEnemyCount(int waveIndex)
    {
        if (waves == null || waveIndex < 0 || waveIndex >= waves.Length) return 0;
        int sum = 0;
        var wave = waves[waveIndex];
        if (wave == null || wave.enemies == null) return 0;
        foreach (var e in wave.enemies) sum += e.amount;
        return sum;
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