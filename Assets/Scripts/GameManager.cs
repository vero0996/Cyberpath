using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    [Header("Estadísticas")]
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

    public void AddAmenaza()
    {
        amenazasDetectadas++;
    }
}
