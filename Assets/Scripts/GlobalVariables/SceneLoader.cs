using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public void GoToGameplay()// Cargar la escena de juego sin cargar una partida guardada
    {
        Time.timeScale = 1f;
        // NO cargar partida guardada
        GuardarJuego.LoadSavedGameRequested = false;

        // borrar save anterior
        if (GuardarJuego.main != null)
            GuardarJuego.main.ClearSavedData();

        PlayerData.ResetMatch();

        // Reiniciar timer y amenazas para empezar una nueva partida limpia
        if (Timer.main != null)
        {
            Timer.main.ReiniciarTimer();
        }

        if (GameManager.main != null)
        {
            GameManager.main.ResetAmenazas();
        }

        SceneManager.LoadScene("GamePlay");
    }

    // Cargar la escena de juego cargando una partida guardada, si existe. Si no, cargar una nueva partida
    public void GoToSavedGame()
    {
        if (!GuardarJuego.main.HasSavedGame())
        {
            Debug.LogWarning(" No saved game found");
            return;
        }
        Time.timeScale = 1f;
        PlayerData.ResetMatch();

        //Cargar partida guardada
        GuardarJuego.LoadSavedGameRequested = true;
        SceneManager.LoadScene("GamePlay");
    }

    // Cargar la escena del menú principal
    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // Cargar la escena de resultados después de la muerte del jugador, mostrando las estadísticas finales
    public void GoToResults()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Achievement");
    }

    
    public void ExitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Splash");
    }
    
    //Cargar escena con los creditos del juego
    public void GoToCredits()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Credits");
    }
}