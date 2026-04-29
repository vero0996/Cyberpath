using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void GoToGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToResults()
    {
        SceneManager.LoadScene("Results"); // o "Results"
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("Splash");
    }
}