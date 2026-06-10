using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public void GameOver()
    {
        GameManager.main.victoria = false;
        APIManager api = FindObjectOfType<APIManager>();
        int tiempo = Mathf.RoundToInt(Timer.main.GetTiempo());
        int amenazas = PlayerData.EnemigosMatados; // ← fix principal
        int waves = PlayerData.WavesCompletadas;
        int totalWaves = 5; // pon tu número real
        int progreso = Mathf.RoundToInt((waves / (float)totalWaves) * 100f);
        int retencion = 0; // perdió = 0%
        api.SendKPI(tiempo, amenazas, progreso, retencion);
    }
}
