using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class Tienda : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TextMeshProUGUI monedaUI;
    [SerializeField] private TextMeshProUGUI puntosUI;

    private void OnGUI()
    {
        monedaUI.text = LevelManager.main.moneda.ToString();
        puntosUI.text = LevelManager.main.puntos.ToString("D7");
    }

    public void SetSeleccion()
    {
        BuildManager.main.SetSelectedDefensa(0);
    }
    
}
