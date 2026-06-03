using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    [Header("Datos jugador")]
    public int moneda => PlayerData.MonedaActual;
    public int puntos => PlayerData.Puntos;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        PlayerData.ResetMatch();
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void AddMoneda(int amount)
    {
        PlayerData.AddMoneda(amount);
    }

    public bool GastarMoneda(int amount)
    {
        if (!PlayerData.GastarMoneda(amount))
        {
            Debug.Log("No tienes suficientes monedas");

            if (MessageManager.main != null)
                MessageManager.main.ShowMessage("No tienes suficientes monedas!");

            return false;
        }

        return true;
    }

    public void AddPuntos(int amount)
    {
        PlayerData.AddPuntos(amount);
    }

    public void GetVida(Jugador jugador)
    {
        
        if (moneda < 1000)
        {
            if (MessageManager.main != null)
                MessageManager.main.ShowMessage("No tienes suficientes monedas!");
            return;
        }

        jugador.Health += 300;
        GastarMoneda(1000);
        if (MessageManager.main != null)
            MessageManager.main.ShowMessage("Curación +300!");
    }
}