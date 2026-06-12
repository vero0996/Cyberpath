using UnityEngine;
using UnityEngine.SceneManagement;  

public class PausarJuego : MonoBehaviour
{
    // Referencia al menú de pausa para mostrar u ocultar
    public GameObject menuPausa;
    public bool juegoPausado = false;

    private void Update()
    {
        // Detectar la tecla Escape para pausar o reanudar el juego
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }
    // Método para reanudar el juego
    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }
    // Método para pausar el juego
    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }
    // Método para volver al menú principal, guardando el estado actual del juego antes de cargar la escena del menú
    public void Menu()
    {
        Time.timeScale = 1f;

        // Guardar estado actual (wave, monedas, defensas, stats)
        if (GuardarJuego.main != null)
        {
            GuardarJuego.main.SaveGame();
            Debug.Log("Estado guardado antes de ir al menú.");
        }

        // Marcar que hay pausa guardada
        PlayerData.SetIsPaused(true);
        var spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
            PlayerData.SetCurrentWave(spawner.currentWave);
        SceneManager.LoadScene("MainMenu");
    }

}
