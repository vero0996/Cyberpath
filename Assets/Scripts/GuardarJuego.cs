using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardarJuego : MonoBehaviour
{
    public static GuardarJuego main;
    public static bool LoadSavedGameRequested = false;

    private const string KEY_SAVE_STATE = "Save_State";
    private const string KEY_SAVE_WAVE = "Save_Wave";
    private const string KEY_SAVE_COINS = "Save_Coins";
    private const string KEY_SAVE_DEFENSES = "Save_Defenses";
    private const string KEY_SAVE_TIEMPO = "Save_Time";

    private void Awake()
    {
        // Singleton: solo la primera instancia persiste
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GuardarJuego: Instancia creada y marcada como DontDestroyOnLoad");
        }
        else if (main != this)
        {
            Debug.Log("GuardarJuego: Duplicado detectado, destruyendo este componente");
            Destroy(this);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

  

    // Borra el save (usar en GameOver / Victory)
    public void ClearSavedData()
    {
        PlayerPrefs.DeleteKey(KEY_SAVE_WAVE);
        PlayerPrefs.DeleteKey(KEY_SAVE_COINS);
        PlayerPrefs.DeleteKey(KEY_SAVE_DEFENSES);
        PlayerPrefs.DeleteKey(KEY_SAVE_TIEMPO);
        PlayerPrefs.SetInt(KEY_SAVE_STATE, 0);
        PlayerPrefs.Save();
        Debug.Log("GuardarJuego: Save data cleared.");
    }

    // Guarda wave actual, monedas y defensas
    public void SaveGame()
    {
        // Guardar wave: intentar obtener EnemySpawner y su wave actual
        int waveToSave = 0;
        var spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
            waveToSave = spawner.currentWave;

        PlayerPrefs.SetInt(KEY_SAVE_WAVE, waveToSave);
        PlayerPrefs.SetInt(KEY_SAVE_COINS, PlayerData.MonedaActual);
        PlayerPrefs.SetFloat(KEY_SAVE_TIEMPO, Timer.main.GetTiempo());

        // Guardar defensas: buscar todas las instancias de Defensas en la escena
        var defenses = FindObjectsOfType<Defensas>();
        List<DefenseSave> list = new List<DefenseSave>();
        foreach (var d in defenses)
        {
            if (d == null) continue;

            // Obtener el nombre del prefab a partir del nombre del GameObject 
            string prefabName = d.gameObject.name.Replace("(Clone)", "").Trim();

            list.Add(new DefenseSave()
            {
                prefabName = prefabName,
                position = d.transform.position,
                rotation = d.transform.rotation
            });
        }

        SaveData data = new SaveData()
        {
            wave = waveToSave,
            coins = PlayerData.MonedaActual,
            defenses = list.ToArray(),
            tiempoJugado = Timer.main.GetTiempo()
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(KEY_SAVE_DEFENSES, json);
        PlayerPrefs.SetInt(KEY_SAVE_STATE, 1); // marca que hay save válido
        PlayerPrefs.Save();

        Debug.Log($"GuardarJuego: Saved game (wave {waveToSave}, coins {PlayerData.MonedaActual}, defenses {list.Count})");
    }

    // On scene loaded, si hay save activo y estamos en la escena de gameplay, restaurar
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "GamePlay")
            return;

        if (LoadSavedGameRequested && HasSavedGame())
        {
            LoadSavedGame();

            // evitar volver a cargar si se cambia de escena
            LoadSavedGameRequested = false;
        }
    }

    public bool HasSavedGame()
    {
        return PlayerPrefs.GetInt(KEY_SAVE_STATE, 0) == 1;
    }

    private void LoadSavedGame()
    {
        if (!HasSavedGame()) return;

        int savedWave = PlayerPrefs.GetInt(KEY_SAVE_WAVE, 0);
        int savedCoins = PlayerPrefs.GetInt(KEY_SAVE_COINS, PlayerData.MonedaActual);
        string json = PlayerPrefs.GetString(KEY_SAVE_DEFENSES, "");

        PlayerData.AddMoneda(savedCoins - PlayerData.MonedaActual); // diferencia para llegar al valor guardado
        Debug.Log($"GuardarJuego: Loading saved game (wave {savedWave}, coins {savedCoins})");

        // Instanciar defensas guardadas
        if (!string.IsNullOrEmpty(json))
        {
            var data = JsonUtility.FromJson<SaveData>(json);
            if (Timer.main != null)
            {
                Timer.main.SetTiempo(data.tiempoJugado);
            }
            if (data != null && data.defenses != null)
            {
                var buildManager = FindObjectOfType<BuildManager>();
                for (int i = 0; i < data.defenses.Length; i++)
                {
                    var ds = data.defenses[i];
                    if (ds == null || string.IsNullOrEmpty(ds.prefabName)) continue;

                    GameObject prefabToSpawn = null;
                    if (buildManager != null)
                    {
                        var torres = buildManager.GetTorres();
                        if (torres != null)
                        {
                            for (int t = 0; t < torres.Length; t++)
                            {
                                if (torres[t] != null && torres[t].prefab != null &&
                                    torres[t].prefab.name == ds.prefabName)
                                {
                                    prefabToSpawn = torres[t].prefab;
                                    break;
                                }
                            }
                        }
                    }

                    if (prefabToSpawn != null)
                    {
                        var inst = Instantiate(prefabToSpawn, ds.position, ds.rotation);
                    }
                    else
                    {
                        Debug.LogWarning($"GuardarJuego: No matching prefab found for saved defense '{ds.prefabName}'.");
                    }
                }
            }
        }

        // Guardar la wave en PlayerPrefs para que EnemySpawner la use al arrancar
        PlayerPrefs.SetInt(KEY_SAVE_WAVE, savedWave);
        PlayerPrefs.Save();
    }

    // Utilidades de serialización
    [System.Serializable]
    private class SaveData
    {
        public int wave;
        public int coins;
        public DefenseSave[] defenses;
        public float tiempoJugado;
    }

    [System.Serializable]
    private class DefenseSave
    {
        public string prefabName;
        public Vector3 position;
        public Quaternion rotation;
    }
}