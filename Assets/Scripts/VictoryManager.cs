using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public void Victoria()
    {
        Debug.Log("VICTORIA EJECUTADA");

        GameManager.main.victoria = true;

        APIManager api = FindObjectOfType<APIManager>();

        int tiempo = Mathf.RoundToInt(Timer.main.GetTiempo());

        api.SendKPI(
            tiempo,
            GameManager.main.amenazasDetectadas,
            100,
            100
        );
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