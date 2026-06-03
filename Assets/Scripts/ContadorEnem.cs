using UnityEngine;

public class ContadorEnem : MonoBehaviour
{
    // Contador centralizado compartido por todos los spawners y enemigos
    public static int Alive { get; private set; }

    public static void Increment()
    {
        Alive++;
    }

    public static void Decrement()
    {
        Alive = Mathf.Max(0, Alive - 1);
    }

    // Forzar recálculo a partir de las instancias existentes en la escena
    public static void RecalculateFromScene()
    {
        Alive = GameObject.FindObjectsOfType<EnemyAI2D>().Length;
    }
}
