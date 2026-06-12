using UnityEngine;
using System;

[System.Serializable] 
public class Torres
{
    public string nombre;// Nombre de la torre
    public int precio;// Costo de la torre
    public GameObject prefab;// Prefab asociado a la torre

    // Constructor de la clase Torres
    public Torres(string _nombre, int _precio, GameObject _prefab)
    {
        // Inicializar los atributos con los valores recibidos
        nombre = _nombre;
        precio = _precio;
        prefab = _prefab;
    }
}