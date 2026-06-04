using TMPro;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [Header("Texts")]

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
    Debug.Log("GameManager = " + GameManager.main);
    
    Debug.Log("1");
    bestScoreText.text = PlayerData.Puntos.ToString();

    Debug.Log("2");
    int minutos = Mathf.FloorToInt(PlayerData.TiempoJugado / 60);
    int segundos = Mathf.FloorToInt(PlayerData.TiempoJugado % 60);

    longestTimeText.text =
        string.Format("{0:00}:{1:00}", minutos, segundos);

    Debug.Log("3");
    totalKillsText.text = PlayerData.EnemigosMatados.ToString();

    Debug.Log("4");
    totalDefensesText.text = PlayerData.DefensasUsadas.ToString();

    Debug.Log("5");
    moneySpentText.text = PlayerData.DineroGastado.ToString();

    Debug.Log("6");
    if (GameManager.main != null)
{
    totalWavesText.text =
    EnemySpawner.WavesCompletadas.ToString();;

    totalRoundsText.text =
        GameManager.main.victoria ? "1" : "0";

    completedWavesText.text = PlayerData.WavesCompletadas.ToString();
}
else
{
    completedWavesText.text = "0";
    totalRoundsText.text = "0";
    totalWavesText.text = "0";
}
}
}