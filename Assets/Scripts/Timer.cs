using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private float tiempoTranscurrido;

    void Update()
    {
        tiempoTranscurrido += Time.deltaTime;

        int minutos = Mathf.FloorToInt(tiempoTranscurrido / 60);
        int segundos = Mathf.FloorToInt(tiempoTranscurrido % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}