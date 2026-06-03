using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public static LevelManager main;
    [Header("Datos jugador")]
    public int moneda => PlayerData.MonedaActual;
    public int puntos => PlayerData.Puntos;

    private void Awake()
    {
        // Inicializa singleton
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayerData.ResetMatch();
    }
    public void AddMoneda(int amount)
    {
        PlayerData.AddMoneda(amount);
    }
    public bool GastarMoneda(int amount)
    {
        if (PlayerData.GastarMoneda(amount))
        {
            return true;
        }

        Debug.Log("No tienes suficientes monedas");
        return false;
    }

    public void AddPuntos(int amount)
    {
        PlayerData.AddPuntos(amount);
    }
    public void GetVida (Jugador jugador)
    {
        if (!PlayerData.GastarMoneda(1000))
        {
            Debug.Log("NO tienes suficientes monedas");
            return;
            
        }
        jugador.Health += 300;
    }
}
