using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer main;

    [SerializeField] private TextMeshProUGUI timerText;

    private float tiempoTranscurrido;

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
        }
    }

    private void Update()
{
    tiempoTranscurrido += Time.deltaTime;
    PlayerData.SetTiempoJugado(tiempoTranscurrido);

    Debug.Log(
        $"Tiempo={tiempoTranscurrido} | TimeScale={Time.timeScale}"
    );

    int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60);
    int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60);

    if (timerText != null)
    {
        timerText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}

    public float GetTiempo()
    {
        return tiempoTranscurrido;
    }

    public string GetTiempoFormateado()
    {
        int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60);
        int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60);

        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void ReiniciarTimer()
    {
        tiempoTranscurrido = 0f;
        PlayerData.SetTiempoJugado(0f);
    }

    public void SetTimerText(TextMeshProUGUI nuevoTexto)
    {
        timerText = nuevoTexto;
    }
}