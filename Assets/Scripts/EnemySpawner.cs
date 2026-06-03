using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public Path path;
    public Wave[] waves;
    private int currentWave = 0;
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    private static int enemiesAlive;
    [SerializeField]
    private int debugEnemiesAlive;



    public void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < waves.Length)
        {
            Wave wave = waves[currentWave];

            yield return new WaitForSeconds(wave.startDelay);

            
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
                enemiesAlive++;

                remaining[randomIndex]--;
                totalEnemies--;
                spawnedEnemies++;
                float currentRate = selectedEnemy.spawnRate;

                if (totalEnemies <= initialEnemyCount / 2)
                {
                    currentRate *= 0.5f;
                }

                yield return new WaitForSeconds(currentRate);
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
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
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

