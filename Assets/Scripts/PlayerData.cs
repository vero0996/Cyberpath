using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData main;

    private const int InitialMoney = 100;

    [SerializeField] private int monedaActual = InitialMoney;
    [SerializeField] private int puntos;
    [SerializeField] private int enemigosMatados;
    [SerializeField] private float tiempoJugado;
    [SerializeField] private int dineroGastado;
    [SerializeField] private int defensasUsadas;

    public static int MonedaActual => main != null ? main.monedaActual : InitialMoney;
    public static int Puntos => main != null ? main.puntos : 0;
    public static int EnemigosMatados => main != null ? main.enemigosMatados : 0;
    public static float TiempoJugado => main != null ? main.tiempoJugado : 0f;
    public static int DineroGastado => main != null ? main.dineroGastado : 0;
    public static int DefensasUsadas => main != null ? main.defensasUsadas : 0;

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

    public static void ResetMatch()
    {
        if (main == null)
        {
            return;
        }

        main.monedaActual = InitialMoney;
        main.puntos = 0;
        main.enemigosMatados = 0;
        main.tiempoJugado = 0f;
        main.dineroGastado = 0;
        main.defensasUsadas = 0;
    }

    public static void SetTiempoJugado(float nuevoTiempo)
    {
        if (main != null)
        {
            main.tiempoJugado = Mathf.Max(0f, nuevoTiempo);
        }
    }

    public static void AddTiempo(float deltaTime)
    {
        if (main != null && deltaTime > 0f)
        {
            main.tiempoJugado += deltaTime;
        }
    }

    public static void AddMoneda(int amount)
    {
        if (main != null)
        {
            main.monedaActual += amount;
        }
    }

    public static bool GastarMoneda(int amount)
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

    public static void AddPuntos(int amount)
    {
        if (main != null)
        {
            main.puntos += amount;
        }
    }

    public static void RegistrarEnemigoMatado()
    {
        if (main != null)
        {
            main.enemigosMatados++;
        }
    }

    public static void RegistrarDefensaUsada()
    {
        if (main != null)
        {
            main.defensasUsadas++;
        }
    }
}