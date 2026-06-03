using TMPro;
using UnityEngine;

public class MostrarPuntaje : MonoBehaviour
{
    public TMP_Text textoPuntaje;

    void Start()
    {
        if (textoPuntaje == null)
        {
            Debug.LogWarning("MostrarPuntaje: textoPuntaje no esta asignado en el Inspector.");
            return;
        }

        textoPuntaje.text = PlayerData.Puntos.ToString();
    }
}