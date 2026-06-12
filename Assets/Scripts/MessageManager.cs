using UnityEngine;
using TMPro;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public static MessageManager main;

    [Header("UI")]// Referencias para el fondo y el texto del mensaje
    [SerializeField] private GameObject background;
    [SerializeField] private TextMeshProUGUI messageText;

    private Coroutine messageCoroutine;// Referencia a la corrutina actual para mostrar el mensaje
    private int messageId = 0;

    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(gameObject);
            return;
        }

        main = this;
    }

    // Método para mostrar un mensaje temporal en pantalla con un estilo específico
    public void ShowMessage(string message, float duration = 2f)
    {
        if (messageText == null)
        {
            Debug.LogError("MessageText no asignado");
            return;
        }
        // Incrementar el ID del mensaje para invalidar cualquier mensaje anterior
        messageId++;
        int currentId = messageId;

        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }
        // Iniciar la corrutina para mostrar el mensaje con el ID actual
        messageCoroutine = StartCoroutine(ShowRoutine(message, duration, currentId));
    }

    // Corrutina que maneja la visualización del mensaje, animando su aparición y desapareciendo después de la duración especificada
    private IEnumerator ShowRoutine(string message, float duration, int id)
    {
        if (background != null)
            background.SetActive(true);

        if (messageText != null)
            messageText.gameObject.SetActive(true);

        // Mensaje estilo sistema
        messageText.text = "<color=#CD163F>[SYSTEM]</color>\n";

        foreach(char c in message)
        {
            messageText.text += c;
            yield return new WaitForSecondsRealtime(0.02f);
        }

        yield return new WaitForSecondsRealtime(duration);

        messageCoroutine = null;
        // Verificar si el ID del mensaje actual coincide con el ID de la corrutina antes de limpiar el mensaje
        if (id != messageId)
            yield break;

        ClearMessage();
    }

    // Método para mostrar un mensaje persistente en pantalla sin duración
    public void ShowPersistentMessage(string message)
    {
        messageId++;

        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }
        if (background != null)
            background.SetActive(true);

        if (messageText != null)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = message;
        }
    }
    // Método para ocultar el mensaje actual
    public void HideMessage()
    {
        ClearMessage();
    }

    // Método privado para limpiar el mensaje de la pantalla
    private void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
            messageText.gameObject.SetActive(false);

            if (background != null)
                background.SetActive(false);
        }
    }
}