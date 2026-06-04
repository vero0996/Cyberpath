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
    public int progreso;        
    public int tasa_retencion; 
}

public class APIManager : MonoBehaviour
{
    string apiUrl = "http://localhost:3000/kpi";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SendKPI(int tiempo, int amenazas, int progreso, int retencion)
    {
        KPIData data = new KPIData();

        data.id_usuario = PlayerPrefs.GetInt("userId", -1);

        if(data.id_usuario == -1)
        {
            Debug.LogError("NO SE RECIBIO USER ID DESDE REACT");
            return;
        }

        data.tiempo_jugado = tiempo;
        data.amenazas_detectadas = amenazas;
        data.progreso = progreso;
        data.tasa_retencion = retencion;

        Debug.Log("=== ENVIANDO KPI ===");
        Debug.Log("Usuario: " + data.id_usuario);
        Debug.Log("Tiempo: " + data.tiempo_jugado);
        Debug.Log("Amenazas: " + data.amenazas_detectadas);

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