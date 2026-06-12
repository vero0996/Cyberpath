using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
   
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Torres[] torre;// Arreglo de torres disponibles para construir
    private int defensaSelected = 0;

    private void Awake()
    {
        main = this;
    }

    private void OnValidate()
    {
        EnsureFirstIsNull();
    }

    // Garantiza que la primera posición del arreglo siempre sea nula
    // para representar "ninguna defensa seleccionada"
    private void EnsureFirstIsNull()
    {
        if (torre == null || torre.Length == 0) return;

        torre[0] = null;
    }

    // Devuelve el arreglo completo de torres
    public Torres[] GetTorres()
    {
        return torre;
    }

    // Devuelve la defensa actualmente seleccionada
    public Torres GetSelectedDefensa()
    {
        // Si el índice es 0, significa que no hay ninguna defensa seleccionada
        if (defensaSelected == 0)
        {
            Debug.Log("No se ha seleccionado ninguna defensa");
            return null;
        }

        return torre[defensaSelected];
    }

    // Cambia la defensa seleccionada
    public void SetSelectedDefensa(int indexTorre)
    {
        defensaSelected = indexTorre;
    }

    // Compra una mejora de vida para el jugador
    public void GetVida(Jugador jugador)
    {
        // Verificar si hay suficientes monedas
        if (LevelManager.main.moneda < 1000)
        {
            // Mostrar mensaje si no hay dinero suficiente
            if (MessageManager.main != null)
                MessageManager.main.ShowMessage("Not enough coins!");

            return;
        }

        // Aumentar la vida del jugador
        jugador.Health += 300;

        // Descontar el costo de la mejora
        LevelManager.main.GastarMoneda(1000);

        // Mostrar mensaje de confirmación
        if (MessageManager.main != null)
            MessageManager.main.ShowMessage("Healing +300!");
    }

    public int GetPrice(GameObject instance)
    {
        // Si el objeto es nulo, regresar un valor inválido
        if (instance == null)
            return -1;

        // Eliminar "(Clone)" del nombre de la instancia
        string instanceName = instance.name.Replace("(Clone)", "").Trim();

        // Verificar que el arreglo de torres exista
        if (torre == null)
            return -1;

        // Buscar la torre correspondiente en el arreglo
        for (int i = 0; i < torre.Length; i++)
        {
            // Comprobar que la torre y su prefab existen
            if (torre[i] != null && torre[i].prefab != null)
            {
                // Comparar nombres
                if (torre[i].prefab.name == instanceName)

                    // Devolver el precio de esa torre
                    return torre[i].precio;
            }
        }
        return -1;
    }
}