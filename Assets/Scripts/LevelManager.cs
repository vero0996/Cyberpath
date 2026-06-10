using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("Datos jugador")]
    public int moneda => PlayerData.MonedaActual;
    public int puntos => PlayerData.Puntos;

    [Header("Enemy money drain")]
    public bool enableEnemyDrain;
    public float drainInterval = 5f;
    public int drainAmount = 1;

    private Coroutine enemyDrainCoroutine;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlayerData.ResetMatch();
        enableEnemyDrain = false;

    }

    private void Update()
    {
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

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

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

    public void GetVida(Jugador jugador)
    {
        
        if (moneda < 1000)
        {
            if (MessageManager.main != null)
                MessageManager.main.ShowMessage("Not enough coins!");
            return;
        }

        jugador.Health += 300;
        GastarMoneda(1000);
        if (MessageManager.main != null)
            MessageManager.main.ShowMessage("Healing +300!");
    }
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