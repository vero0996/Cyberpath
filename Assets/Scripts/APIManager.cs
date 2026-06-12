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
    // URL del endpoint que recibe los KPI
    string apiUrl = "http://localhost:3000/kpi";

    void Awake()
    {
        // Hace que este objeto no sea destruido al cambiar de escena
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    // Función que prepara los datos y comienza el envío
    public void SendKPI(int tiempo, int amenazas, int progreso, int retencion)
    {
        // Crear objeto con la información a enviar
        KPIData data = new KPIData();

        // Obtener el id del usuario guardado en PlayerPrefs
        data.id_usuario = PlayerPrefs.GetInt("userId", -1);

        // Si no existe el ID del usuario, cancelar el envío
        if (data.id_usuario == -1)
        {
            Debug.LogError("NO SE RECIBIO USER ID DESDE REACT");
            return;
        }

        // Asignar los valores recibidos
        data.tiempo_jugado = tiempo;
        data.amenazas_detectadas = amenazas;
        data.progreso = progreso;
        data.tasa_retencion = retencion;

        // Mostrar información de depuración
        Debug.Log("=== ENVIANDO KPI ===");
        Debug.Log("Usuario: " + data.id_usuario);
        Debug.Log("Tiempo: " + data.tiempo_jugado);
        Debug.Log("Amenazas: " + data.amenazas_detectadas);

        // Iniciar la corrutina que realiza la petición POST
        StartCoroutine(PostKPI(data));
    }

    // Corrutina encargada de enviar los datos al servidor
    IEnumerator PostKPI(KPIData data)
    {
        // Convertir el objeto a formato JSON
        string json = JsonConvert.SerializeObject(data);

        Debug.Log("📤 Enviando KPI: " + json);

        // Crear una petición HTTP POST
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");

        // Convertir el JSON en un arreglo de bytes
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        // Configurar los datos enviados y la respuesta esperada
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Indicar que se enviará contenido JSON
        request.SetRequestHeader("Content-Type", "application/json");

        // Esperar a que termine la petición
        yield return request.SendWebRequest();

        // Mostrar el código de respuesta y el contenido recibido
        Debug.Log("📡 Response Code: " + request.responseCode);
        Debug.Log("📨 Response: " + request.downloadHandler.text);

        // Verificar si la petición fue exitosa
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