using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public void Victoria()
    {
        GameManager.main.victoria = true;
        APIManager api = FindObjectOfType<APIManager>();
        int tiempo = Mathf.RoundToInt(Timer.main.GetTiempo());
        int amenazas = PlayerData.EnemigosMatados; // ← fix principal
        int progreso = 100; // ganó = 100%
        int retencion = 100; // completó = 100%
        api.SendKPI(tiempo, amenazas, progreso, retencion);
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}