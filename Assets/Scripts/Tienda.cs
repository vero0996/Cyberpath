using UnityEngine;
using TMPro;

public class Tienda : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TextMeshProUGUI monedaUI;
    [SerializeField] private TextMeshProUGUI puntosUI;

    private void Update()
    {
        monedaUI.text = PlayerData.MonedaActual.ToString();
        puntosUI.text = PlayerData.Puntos.ToString("D7");
    }

    public void ComprarDefensa(int costo)
    {
        if (!LevelManager.main.GastarMoneda(costo))
        {
            return;
        }

        BuildManager.main.SetSelectedDefensa(0);
    }
}