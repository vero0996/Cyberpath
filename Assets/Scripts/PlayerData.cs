using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData main;

    // Valores iniciales para las estadísticas del jugador
    private const int InitialMoney = 150;
    
    [SerializeField] private int monedaActual = InitialMoney;
    [SerializeField] private int puntos;// puntos acumulados por el jugador
    [SerializeField] private int enemigosMatados;
    [SerializeField] private float tiempoJugado;
    [SerializeField] private int dineroGastado;// cantidad total de dinero gastado por el jugador
    [SerializeField] private int defensasUsadas;// cantidad de defensas colocadas por el jugador

    // Propiedades para acceder a las estadísticas del jugador desde otras clases
    [Header("Estado Pausa")]
    [SerializeField] private bool isPaused = false;
    [SerializeField] private int currentWavePaused = 0;

    // Propiedades públicas para acceder a las estadísticas del jugador
    public static int MonedaActual => main != null ? main.monedaActual : InitialMoney;
    public static int Puntos => main != null ? main.puntos : 0;// puntos acumulados por el jugador
    public static int EnemigosMatados => main != null ? main.enemigosMatados : 0;// cantidad total de enemigos eliminados por el jugador
    public static float TiempoJugado => main != null ? main.tiempoJugado : 0f;// tiempo total jugado por el jugador en segundos
    public static int DineroGastado => main != null ? main.dineroGastado : 0;// cantidad total de dinero gastado por el jugador
    public static int DefensasUsadas => main != null ? main.defensasUsadas : 0;// cantidad total de defensas colocadas por el jugador
    public static int WavesCompletadas => main != null ? main.wavesCompletadas : 0;// cantidad total de oleadas completadas por el jugador
    
    public static bool IsPaused => main != null ? main.isPaused : false;// indica si el juego está actualmente en pausa
    public static int CurrentWavePaused => main != null ? main.currentWavePaused : 0;// índice de la oleada en la que se guardó la pausa, si es que hay una pausa guardada


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
        }
    }

    // Reinicia las estadísticas del jugador al inicio de una nueva partida o al cargar un nivel, pero solo si no hay una pausa guardada
    public static void ResetMatch()
    {
        if (main == null)
        {
            return;
        }
        // Solo resetear si NO hay pausa guardada
        if (main.isPaused)
        {
            Debug.Log("PlayerData: Match no se resetea porque hay pausa guardada.");
            return;
        }
        // Reiniciar todas las estadísticas a sus valores iniciales
        main.monedaActual = InitialMoney;
        main.puntos = 0;
        main.enemigosMatados = 0;
        main.tiempoJugado = 0f;
        main.dineroGastado = 0;
        main.defensasUsadas = 0;
        main.wavesCompletadas = 0;
        main.isPaused = false;
        main.currentWavePaused = 0;
    }
   
    public static void SetTiempoJugado(float nuevoTiempo)// Método para actualizar el tiempo jugado
    {
        if (main != null)
        {
            main.tiempoJugado = Mathf.Max(0f, nuevoTiempo);
        }
    }

    public static void AddTiempo(float deltaTime)// Método para incrementar el tiempo jugado, utilizado en el Update del LevelManager
    {
        if (main != null && deltaTime > 0f)
        {
            main.tiempoJugado += deltaTime;
        }
    }

    public static void AddMoneda(int amount)// Método para agregar monedas al jugador
    {
        if (main != null)
        { 
            main.monedaActual += amount;
            Debug.Log($"Moneda AGREGADA");
        }
        
    }

    public static bool GastarMoneda(int amount)// Método para gastar monedas al solicitar una compra
    {
        if (amount <= 0)
        {
            return true;
        }

        if (main == null || amount > main.monedaActual)
        {
            return false;
        }

        main.monedaActual -= amount;
        main.dineroGastado += amount;
        return true;
    }

    public static int DeductMoneda(int amount)// Método para deducir monedas del jugador
    {
        if (main == null || amount <= 0) return 0;

        int toDeduct = Mathf.Min(main.monedaActual, amount);
        main.monedaActual -= toDeduct;
        main.dineroGastado += toDeduct;
        return toDeduct;
    }

    public static void AddPuntos(int amount)// Método para agregar puntos al jugador
    {
        if (main != null)
        {
            main.puntos += amount;
        }
    }

    public static void RegistrarEnemigoMatado()// Método para registrar que el jugador ha matado a un enemigo
    {
        if (main != null)
        {
            main.enemigosMatados++;
        }
    }

    public static void RegistrarDefensaUsada()// Método para registrar que el jugador ha colocado una defensa
    {
        if (main != null)
        {
            main.defensasUsadas++;
        }
    }

    [SerializeField] private int wavesCompletadas;

    public static void SetWavesCompletadas(int wave)// Método para actualizar la cantidad de oleadas completadas por el jugador
    {
        if(main != null)
        {
            main.wavesCompletadas = wave;
        }
    }

    // manejar pausa
    public static void SetIsPaused(bool paused)
    {
        if (main != null)
            main.isPaused = paused;
    }

    public static void SetCurrentWave(int waveIndex)// Método para actualizar el índice de la oleada en la que se guardó la pausa
    {
        if (main != null)
            main.currentWavePaused = waveIndex;
    }
}