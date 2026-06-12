using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    
    public void Victoria()
    {
        // Marcar la partida como una victoria
        GameManager.main.victoria = true;

        // Buscar el objeto encargado de la comunicación con la API
        APIManager api = FindObjectOfType<APIManager>();

        // Obtener el tiempo total jugado y convertirlo a entero
        int tiempo = Mathf.RoundToInt(Timer.main.GetTiempo());

        // Obtener el número total de enemigos eliminados
        int amenazas = PlayerData.EnemigosMatados;

        // Al ganar la partida, el progreso es del 100%
        int progreso = 100;

        // Al completar la partida, la retención también es del 100%
        int retencion = 100;

        // Enviar los KPI al servidor
        api.SendKPI(tiempo, amenazas, progreso, retencion);
    }

    // Regresar al menú principal
    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Reiniciar el juego cargando nuevamente la escena de gameplay
    public void PlayAgain()
    {
        SceneManager.LoadScene("GamePlay");
    }

    // Salir del juego
    public void Quit()
    {
        Application.Quit();

        // Si se está ejecutando desde el editor de Unity,
        // detener el modo Play
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}