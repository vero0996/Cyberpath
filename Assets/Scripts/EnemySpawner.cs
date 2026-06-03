using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public Path path;
    public Wave[] waves;
    private int currentWave = 0;
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    private static int enemiesAlive;
    [SerializeField]
    public int debugEnemiesAlive;

    [Header("UI")]
    public TextMeshProUGUI waveMessage; // asignar en Inspector
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
                StartCoroutine(ShowWaveStart(currentWave+1, waveMessageDuration));
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
            
            // Mientras queden enemigos por generar
            while (totalEnemies > 0)
            {
               
                System.Collections.Generic.List<int> availableTypes =
                    new System.Collections.Generic.List<int>();

               
                for (int i = 0; i < wave.enemies.Length; i++)
                {
                    // Generar una lista de tipos disponibles
                    bool EnemigosDisponibles = remaining[i] > 0;
                    bool Desbloqueo = spawnedEnemies >= wave.enemies[i].unlockAfter;

                    if (EnemigosDisponibles && Desbloqueo)
                    {
                        availableTypes.Add(i);
                    }
                }

                if (availableTypes.Count == 0)
                {
                    yield return null;
                    continue;
                }

                // Elegir uno aleatoriamente
                int randomIndex = availableTypes[ Random.Range(0, availableTypes.Count)];

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

            // Ronda completada: mostrar mensaje en pantalla
            if (waveMessage != null)
            {
                StartCoroutine(ShowWaveComplete(currentWave + 1, waveMessageDuration));
            }

            yield return new WaitForSeconds(wave.timeBetweenWaves);

            currentWave++;
        }
    }
    private IEnumerator ShowWaveStart(int waveNumber, float duration)
    {
        if (waveMessage == null) yield break;

        waveMessage.gameObject.SetActive(true);
        waveMessage.text = $"Inicio ronda {waveNumber} ";
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

   
}

[System.Serializable]
public class  Enemy
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

