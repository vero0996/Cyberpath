using UnityEngine;
using UnityEngine.SceneManagement;  

public class PausarJuego : MonoBehaviour
{
    public GameObject menuPausa;
    public bool juegoPausado = false;

    private void Update()
    {
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
    public void Reanudar()
    {
        menuPausa.SetActive(false);
        Time.timeScale = 1f;
        juegoPausado = false;
    }
    public void Pausar()
    {
        menuPausa.SetActive(true);
        Time.timeScale = 0f;
        juegoPausado = true;
    }
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
