using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("Datos jugador")]// Propiedades para acceder a los datos del jugador
    public int moneda => PlayerData.MonedaActual;
    public int puntos => PlayerData.Puntos;

    [Header("Enemy money drain")]// Configuración para el sistema de drenaje de monedas basado en la cantidad de enemigos vivos
    public bool enableEnemyDrain;
    public float drainInterval = 5f;
    public int drainAmount = 1;

    private Coroutine enemyDrainCoroutine;
    // número inicial de enemigos de la ronda actual 
    private int currentWaveInitialEnemies = 0;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (main != this)
        {
            Destroy(gameObject);
            return;
        }
    }
   
    private void Start()
    {
        PlayerData.ResetMatch();// Reiniciar datos del jugador al iniciar el nivel
        enableEnemyDrain = false;

        // sincronizar con el spawner si ya existe
        var spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
            currentWaveInitialEnemies = spawner.GetInitialEnemyCount(0);

    }

    // Método para manejar el evento de inicio de una nueva oleada, actualizando el número inicial de enemigos
    private void HandleWaveStarted(int waveIndex, int initialEnemyCount)
    {
        currentWaveInitialEnemies = initialEnemyCount;
        Debug.Log($"LevelManager: Wave {waveIndex} started with {initialEnemyCount} enemies.");
    }

    private void Update()
    {
        // Verificar si el número de enemigos vivos supera el umbral para activar el drenaje de monedas
        if (ContadorEnem.Alive >= 15)
        {
            if (!enableEnemyDrain)
            {
                enableEnemyDrain = true;
                StartEnemyDrain();
            }
        }
        else
        {
            if (enableEnemyDrain)
            {
                enableEnemyDrain = false;
                StopEnemyDrain();
            }
        }
    }

    // Métodos para agregar y gastar monedas, así como para agregar puntos
    public void AddMoneda(int amount)
    {
        PlayerData.AddMoneda(amount);
    }

    public bool GastarMoneda(int amount)
    {
        if (!PlayerData.GastarMoneda(amount))
        {
            Debug.Log("No tienes suficientes monedas");

            if (MessageManager.main != null)
                MessageManager.main.ShowMessage("Not enough coins!");

            return false;
        }

        return true;
    }

    public void AddPuntos(int amount)
    {
        PlayerData.AddPuntos(amount);
    }

    // Métodos para iniciar y detener la rutina de drenaje de monedas basada en enemigos vivos
    public void StartEnemyDrain()
    {
        if (enemyDrainCoroutine == null)
            enemyDrainCoroutine = StartCoroutine(EnemyDrainRoutine());
    }

    public void StopEnemyDrain()
    {
        if (enemyDrainCoroutine != null)
        {
            StopCoroutine(enemyDrainCoroutine);
            enemyDrainCoroutine = null;
        }
    }
    // Rutina que se ejecuta periódicamente para deducir monedas del jugador mientras haya muchos enemigos vivos    
    private IEnumerator EnemyDrainRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(drainInterval);

            int deducted = PlayerData.DeductMoneda(drainAmount);
            

            Debug.Log($"Enemy drain: -{deducted} coins. Remaining: {moneda}");
        }
    }

}