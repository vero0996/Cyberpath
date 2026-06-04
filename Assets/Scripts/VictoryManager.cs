using UnityEngine;

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
}