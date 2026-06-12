using TMPro;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [Header("Texts")]

    //  Estadísticas del jugador
    public TMP_Text bestScoreText;
    public TMP_Text longestTimeText;
    public TMP_Text totalKillsText;
    public TMP_Text totalDefensesText;

    public TMP_Text completedWavesText;
    public TMP_Text moneySpentText;
    public TMP_Text totalRoundsText;
    public TMP_Text totalWavesText;

    void Start()
    {
        LoadStats();
    }

    void LoadStats()
    {
        // Verificar si existe una instancia de GameManager
        Debug.Log("GameManager = " + GameManager.main);

        // Mostrar los puntos obtenidos por el jugador
        Debug.Log("1");
        bestScoreText.text = PlayerData.Puntos.ToString();

        // Convertir el tiempo jugado de segundos a formato MM:SS
        Debug.Log("2");
        int minutos = Mathf.FloorToInt(PlayerData.TiempoJugado / 60);
        int segundos = Mathf.FloorToInt(PlayerData.TiempoJugado % 60);

        // Mostrar el tiempo total jugado
        longestTimeText.text =
            string.Format("{0:00}:{1:00}", minutos, segundos);

        // Mostrar la cantidad total de enemigos eliminados
        Debug.Log("3");
        totalKillsText.text = PlayerData.EnemigosMatados.ToString();

        // Mostrar la cantidad total de defensas utilizadas
        Debug.Log("4");
        totalDefensesText.text = PlayerData.DefensasUsadas.ToString();

        // Mostrar la cantidad total de dinero gastado
        Debug.Log("5");
        moneySpentText.text = PlayerData.DineroGastado.ToString();

        Debug.Log("6");

        if (GameManager.main != null)
        {
            // Mostrar el número de oleadas completadas en la partida
            totalWavesText.text = EnemySpawner.WavesCompletadas.ToString();

            // Mostrar si la partida fue ganada (1) o perdida (0)
            totalRoundsText.text =
                GameManager.main.victoria ? "1" : "0";

            // Mostrar el total acumulado de oleadas completadas
            completedWavesText.text = PlayerData.WavesCompletadas.ToString();
        }
        else
        {
            // Si no existe GameManager, inicializar los textos con 0
            completedWavesText.text = "0";
            totalRoundsText.text = "0";
            totalWavesText.text = "0";
        }
    }
}