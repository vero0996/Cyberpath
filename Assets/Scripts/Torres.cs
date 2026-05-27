using UnityEngine;
using System;

[System.Serializable]
public class Torres
{
    public string nombre;
    public int precio;
    public GameObject prefab;
    
    public Torres(string _nombre, int _precio, GameObject _prefab)
    {
        nombre = _nombre;
        precio = _precio;
        prefab = _prefab;
    }
}
