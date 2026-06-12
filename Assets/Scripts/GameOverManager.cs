using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public void GameOver()// Método para manejar el evento de Game Over
    {
        GameManager.main.victoria = false;
        APIManager api = FindObjectOfType<APIManager>();// Obtener la instancia del APIManager para enviar las estadísticas del juego
        int tiempo = Mathf.RoundToInt(Timer.main.GetTiempo());
        int amenazas = PlayerData.EnemigosMatados; // ← fix principal
        int waves = PlayerData.WavesCompletadas;
        int totalWaves = 5; // pon tu número real
        int progreso = Mathf.RoundToInt((waves / (float)totalWaves) * 100f);// Calcular el progreso como porcentaje de oleadas completadas
        int retencion = 0; // perdió = 0%
        api.SendKPI(tiempo, amenazas, progreso, retencion);// Enviar las estadísticas del juego al backend
    }
}
