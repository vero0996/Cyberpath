using UnityEngine;
using System.Collections;
using System.Collections.Generic;   
using TMPro;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    
    public Path path;// Referencia al componente Path para que los enemigos sigan el camino definido
    public Wave[] waves;// Arreglo de oleadas que define qué enemigos se generan, en qué cantidad y con qué frecuencia
    public int currentWave = 0;

    [SerializeField]
    private int debugEnemiesAlive;

    [Header("UI")]// Referencias para mostrar mensajes de inicio y finalización de oleadas
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
        StartCoroutine(SpawnWaves());// Iniciar la generación de oleadas
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < waves.Length)
        {
            Wave wave = waves[currentWave];// Obtener la configuración de la oleada actual

            yield return new WaitForSeconds(wave.startDelay);
           

            if (waveMessage != null)// Mostrar mensaje de inicio de oleada
            {
                StartCoroutine(ShowWaveStart(currentWave + 1, waveMessageDuration));
                yield return new WaitForSeconds(waveMessageDuration);
            }

            int[] remaining = new int[wave.enemies.Length];

            // Inicializar el arreglo de enemigos restantes para la oleada actual
            for (int i = 0; i < wave.enemies.Length; i++)
            {
                remaining[i] = wave.enemies[i].amount;
            }

            int totalEnemies = 0;

            // Calcular el número total de enemigos en la oleada actual
            foreach (Enemy enemy in wave.enemies)
            {
                totalEnemies += enemy.amount;
            }

            int spawnedEnemies = 0;
            int initialEnemyCount = totalEnemies;

            // Generar enemigos mientras haya enemigos restantes en la oleada
            while (totalEnemies > 0)
            {
                // Crear una lista de índices de tipos de enemigos disponibles para generar
                List<int> availableTypes = new List<int>();

                // Verificar qué tipos de enemigos aún tienen unidades restantes y cumplen con el requisito de desbloqueo
                for (int i = 0; i < wave.enemies.Length; i++)
                {
                    bool enemigosDisponibles = remaining[i] > 0;
                    bool desbloqueo = spawnedEnemies >= wave.enemies[i].unlockAfter;

                    if (enemigosDisponibles && desbloqueo)
                    {
                        availableTypes.Add(i);
                    }
                }
                // Si no hay tipos de enemigos disponibles para generar, esperar un frame y volver a verificar
                if (availableTypes.Count == 0)
                {
                    yield return null;
                    continue;
                }

                // Seleccionar aleatoriamente un tipo de enemigo de los disponibles
                int randomIndex =
                    availableTypes[Random.Range(0, availableTypes.Count)];

                Enemy selectedEnemy = wave.enemies[randomIndex];

                SpawnEnemy(selectedEnemy.enemyPrefab);

                // Actualizar el conteo de enemigos restantes y generados
                remaining[randomIndex]--;
                totalEnemies--;
                spawnedEnemies++;

                float currentRate = selectedEnemy.spawnRate;

                // Si el número de enemigos restantes es la mitad o menos del número inicial, aumentar la tasa de generación
                if (totalEnemies <= initialEnemyCount / 2)
                {
                    currentRate *= 0.5f;
                }
                // Si no quedan enemigos por generar, revertir la tasa de generación al valor original
                if (totalEnemies == 0)
                {
                    currentRate /= 0.5f;
                }

                yield return new WaitForSeconds(currentRate);
            }
            // Esperar a que todos los enemigos de la oleada actual sean eliminados antes de pasar a la siguiente oleada
            ContadorEnem.RecalculateFromScene();
            debugEnemiesAlive = ContadorEnem.Alive;

            while (ContadorEnem.Alive > 0)
            {
                yield return null;
            }
            // Incrementar el número de oleadas completadas en PlayerData
            PlayerData.SetWavesCompletadas(currentWave + 1);

            // Mostrar mensaje de finalización de oleada
            if (waveMessage != null)
            {
                StartCoroutine(ShowWaveComplete(currentWave + 1, waveMessageDuration));
            }
            // Si la oleada actual es la última, mostrar mensaje de victoria, limpiar datos guardados y cargar escena de victoria
            if ( currentWave == waves.Length - 1)
            {
                StartCoroutine(ShowVictory(waveMessageDuration));
                yield return new WaitForSeconds(waveMessageDuration);
                GuardarJuego guardarJuego = FindObjectOfType<GuardarJuego>();
                guardarJuego.ClearSavedData();  
                SceneManager.LoadScene("VICTORY");
            }
            yield return new WaitForSeconds(wave.timeBetweenWaves);

            // Cada 2 oleadas, aumentar la dificultad de los enemigos incrementando su velocidad y puntos de vida
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
    // Método para generar un enemigo en la posición del spawner y asignarle el camino a seguir
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

    // Métodos para mostrar mensajes de inicio y finalización de oleadas,
    // así como mensaje de victoria, utilizando corrutinas para controlar la duración de los mensajes en pantalla
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
    // Obtener numero inicial de enemigos de una wave
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
// Clase que representa un tipo de enemigo, con su prefab, cantidad a generar, tasa de generación y requisito de desbloqueo
[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int amount;
    public float spawnRate;

    [Header("Desbloqueo")]
    public int unlockAfter;
}
// Clase que representa una oleada de enemigos, con un arreglo de tipos de enemigos,
// tiempo entre oleadas y retraso inicial antes de comenzar la oleada
[System.Serializable]
public class Wave
{
    public Enemy[] enemies;
    public float timeBetweenWaves;
    public float startDelay;
}