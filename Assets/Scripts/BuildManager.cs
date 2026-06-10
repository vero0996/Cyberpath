using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Torres[] torre;

    private int defensaSelected = 0;
    

    private void Awake()
    {
        main = this;
    }

    // Se ejecuta en el editor cuando cambias algo en el inspector
    private void OnValidate()
    {
        EnsureFirstIsNull();
    }

    private void EnsureFirstIsNull()
    {
        if (torre == null || torre.Length == 0) return;
        torre[0] = null;
    }
    // getter para acceder al array de torres
    public Torres[] GetTorres()
    {
        return torre;
    }

    public Torres GetSelectedDefensa()
    {
        if (defensaSelected == 0)
        {
            Debug.Log("No se ha seleccionado ninguna defensa");
            
            return null;
        }
        return torre[defensaSelected];
    }

    public void SetSelectedDefensa(int indexTorre)
    {
        defensaSelected = indexTorre ;
    }

    public int GetPrice(GameObject instance)
    {
        if (instance == null) return -1;

        string instanceName = instance.name.Replace("(Clone)", "").Trim();

        if (torre == null) return -1;

        for (int i = 0; i < torre.Length; i++)
        {
            if (torre[i] != null && torre[i].prefab != null)
            {
                if (torre[i].prefab.name == instanceName)
                    return torre[i].precio;
            }
        }

        return -1;
    }
}