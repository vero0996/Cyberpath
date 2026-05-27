using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class Tienda : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TextMeshProUGUI monedaUI;

    private void OnGUI()
    {
        monedaUI.text = LevelManager.main.moneda.ToString();
    }

    public void SetSeleccion()
    {
        BuildManager.main.SetSelectedDefensa(0);
    }
    
}
