using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public void Victoria()
    {
        GameManager.main.victoria = true;

        APIManager api = FindObjectOfType<APIManager>();

        int tiempo =
            Mathf.RoundToInt(Timer.main.GetTiempo());

        int amenazas =
            GameManager.main.amenazasDetectadas;

        int progreso = 100;

        int retencion = 100;

        api.SendKPI(
            tiempo,
            amenazas,
            progreso,
            retencion
        );
    }
}