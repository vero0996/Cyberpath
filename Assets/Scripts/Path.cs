using UnityEngine;

public class Path : MonoBehaviour
{
    public Transform[] waypoints;// Arreglo de puntos de camino que los enemigos seguirán

    void Awake()
    {
        // Inicializar el arreglo de waypoints con los hijos del objeto Path
        waypoints = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }
}