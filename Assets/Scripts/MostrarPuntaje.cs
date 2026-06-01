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

        if (LevelManager.main == null)
        {
            // This happens when starting directly from GameOver in the editor.
            textoPuntaje.text = "0";
            return;
        }

        textoPuntaje.text = LevelManager.main.puntos.ToString();
    }
}