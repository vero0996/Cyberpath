using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    
    public static Timer main;

    [SerializeField] private TextMeshProUGUI timerText;

    // Tiempo transcurrido desde que comenzó la partida
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
            // Si ya existe un Timer, destruir el duplicado
            Destroy(gameObject);
        }

        // Obtener el tiempo actual    
        GetTiempo();
    }

    private void Update()
    {
        // Incrementar el tiempo usando el tiempo transcurrido entre frames
        tiempoTranscurrido += Time.deltaTime;

        // Guardar el tiempo en PlayerData
        PlayerData.SetTiempoJugado(tiempoTranscurrido);

        // Mostrar información de depuración
        Debug.Log(
            $"Tiempo={tiempoTranscurrido} | TimeScale={Time.timeScale}"
        );

        // Convertir segundos a minutos y segundos
        int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60);
        int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60);

        // Actualizar el texto del temporizador si existe
        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }

    // Devuelve el tiempo actual en segundos
    public float GetTiempo()
    {
        return tiempoTranscurrido;
    }

    // Establece manualmente el tiempo del temporizador
    public void SetTiempo(float tiempo)
    {
        tiempoTranscurrido = tiempo;
    }

    // Devuelve el tiempo con formato MM:SS
    public string GetTiempoFormateado()
    {
        int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60);
        int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60);

        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    // Reinicia completamente el temporizador
    public void ReiniciarTimer()
    {
        tiempoTranscurrido = 0f;

        // Reiniciar también el valor almacenado en PlayerData
        PlayerData.SetTiempoJugado(0f);
    }

    
    public void SetTimerText(TextMeshProUGUI nuevoTexto)
    {
        timerText = nuevoTexto;
    }
}