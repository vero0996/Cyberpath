using TMPro;
using UnityEngine;

public class MostrarPuntaje : MonoBehaviour
{
    // Referencia al componente de texto para mostrar el puntaje
    public TMP_Text textoPuntaje;

    void Start()
    {
        // Verificar que la referencia al texto esté asignada
        if (textoPuntaje == null)
        {
            Debug.LogWarning("MostrarPuntaje: textoPuntaje no esta asignado en el Inspector.");
            return;
        }

        textoPuntaje.text = PlayerData.Puntos.ToString();
    }
}