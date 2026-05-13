using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public Path path;
    public Wave[] waves;
    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }
    
    IEnumerator SpawnWaves() 
    {
        while (currentWave < waves.Length)
        { Wave wave = waves[currentWave];

            //grupos de enemigos
            foreach (Enemy group in wave.enemies)
            {
                for (int i = 0; i < group.amount; i++)
                {
                    SpawnEnemy(group.enemyPrefab);

                    float currentRate = group.spawnRate;

                    if (i > group.amount / 2)
                    {
                        currentRate *= 0.6f;
                    }
                    yield return new WaitForSeconds(currentRate);
                }
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
}

[System.Serializable]
public class  Enemy
{
    public GameObject enemyPrefab;
    public int amount;
    public float spawnRate;
}
[System.Serializable]
public class Wave
{
    public Enemy[] enemies;
    public float timeBetweenWaves;

}

