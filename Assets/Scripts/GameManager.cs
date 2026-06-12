using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    [Header("Estadísticas")]// Variables para rastrear las estadísticas del juego
    public int amenazasDetectadas;
    public int nivelCompletado;
    public bool victoria;

    private void Awake()
    {
        if(main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Método para agregar una amenaza detectada
    public void AddAmenaza()
    {
        amenazasDetectadas++;
    }
    // Método para completar un nivel
    public void ResetAmenazas()
    {
        amenazasDetectadas = 0;
    }
}
