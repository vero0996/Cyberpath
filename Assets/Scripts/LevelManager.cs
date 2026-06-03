using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    
    public void Menu()
    {
        Time.timeScale = 1f;
        moneda = 100;
        puntos = 00000;
        SceneManager.LoadScene("MainMenu");
    }
    public static LevelManager main;
    [Header("Datos jugador")]
    public int moneda;
    public int puntos;

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
            return;
        }
    }
    private void Start()
    {
        moneda = 100;
        puntos = 00000;

    }
    public void AddMoneda(int amount)
    {
        moneda += amount;
    }
    public bool GastarMoneda(int amount)
    {
        if (amount <= moneda)
        {
            moneda -= amount;
            return true;
        }
        else 
        { 
            Debug.Log("No tienes suficientes monedas");
            if (MessageManager.main != null)
            {
                MessageManager.main.ShowMessage("Not enough money!");
            }

            return false;
        }
    }

    public void AddPuntos(int amount)
    {
        puntos += amount;
    }
    public void GetVida (Jugador jugador)
    {
        if (moneda <= 999)
        {
            Debug.Log("NO tienes suficientes monedas");
            if (MessageManager.main != null)
            {
                MessageManager.main.ShowMessage("Not enough money!");
            }

            return;
            
        }
        jugador.Health += 300;
        GastarMoneda(1000);
    }
}
