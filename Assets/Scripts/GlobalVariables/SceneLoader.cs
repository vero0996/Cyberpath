using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GoToGameplay()
    {
        Time.timeScale = 1f;
        // NO cargar partida guardada
        GuardarJuego.LoadSavedGameRequested = false;

        // borrar save anterior
        if (GuardarJuego.main != null)
            GuardarJuego.main.ClearSavedData();

        PlayerData.ResetMatch();

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

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

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

    public void GoToCredits()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Credits");
    }
}