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

    public int moneda;

    private void Awake()
    {
        // Inicializa singleton
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject); // opcional si quieres que persista entre escenas
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        moneda = 100;
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
        else { Debug.Log("No tienes suficientes monedas");
            return false;
        }
    }
}
