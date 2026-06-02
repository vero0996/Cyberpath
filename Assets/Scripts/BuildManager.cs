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

    public Torres GetSelectedDefensa()
    {
        torre[0].nombre = null;
        torre[0].precio = 0;
        torre[0].prefab = null;
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