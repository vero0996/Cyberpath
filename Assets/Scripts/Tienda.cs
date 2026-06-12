using UnityEngine;
using TMPro;

public class Tienda : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TextMeshProUGUI monedaUI;
    [SerializeField] private TextMeshProUGUI puntosUI;

    private void Update()
    {
        // Actualizar continuamente la cantidad de monedas mostrada en pantalla
        monedaUI.text = PlayerData.MonedaActual.ToString();

        // Mostrar la puntuación con 7 dígitos, rellenando con ceros a la izquierda
        puntosUI.text = PlayerData.Puntos.ToString("D7");
    }

    // Método llamado al comprar una defensa desde la tienda
    public void ComprarDefensa(int costo)
    {
        // Intentar descontar las monedas necesarias
        // Si no hay suficiente dinero, salir del método
        if (!LevelManager.main.GastarMoneda(costo))
        {
            return;
        }

        // Reiniciar la selección de defensa 
        BuildManager.main.SetSelectedDefensa(0);
    }
}