using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public void GameOver()
    {
        Debug.Log("GAME OVER EJECUTADO");
        GameManager.main.victoria = false;

        APIManager api = FindObjectOfType<APIManager>();

        int tiempo = Mathf.RoundToInt(Timer.main.GetTiempo());

        api.SendKPI(
            tiempo,
            GameManager.main.amenazasDetectadas,
            100,
            100
        );
    }
}
