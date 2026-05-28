using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using System.Text;

[System.Serializable]
public class KPIData
{
    public int id_usuario;
    public int tiempo_jugado;
    public int amenazas_detectadas;
    public int progreso;        // 🔧 cambiado a int
    public int tasa_retencion;  // 🔧 cambiado a int
}

public class APIManager : MonoBehaviour
{
    // 🔧 IMPORTANTE: si es build o celular, NO uses localhost
    string apiUrl = "http://localhost:3000/kpi";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // 🔥 ahora recibe datos reales del juego
    public void SendKPI(int tiempo, int amenazas, int progreso, int retencion)
    {
        KPIData data = new KPIData();

        // 🔧 USER REAL (por ahora PlayerPrefs)
        data.id_usuario = PlayerPrefs.GetInt("userId", 1);

        data.tiempo_jugado = tiempo;
        data.amenazas_detectadas = amenazas;
        data.progreso = progreso;
        data.tasa_retencion = retencion;

        StartCoroutine(PostKPI(data));
    }

    IEnumerator PostKPI(KPIData data)
    {
        string json = JsonConvert.SerializeObject(data);

        Debug.Log("📤 Enviando KPI: " + json);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        // 🔥 DEBUG REAL COMPLETO
        Debug.Log("📡 Response Code: " + request.responseCode);
        Debug.Log("📨 Response: " + request.downloadHandler.text);

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ KPI enviado correctamente");
        }
        else
        {
            Debug.LogError("❌ Error enviando KPI: " + request.error);
        }
    }
}