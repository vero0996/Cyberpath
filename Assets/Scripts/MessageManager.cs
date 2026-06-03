using UnityEngine;
using TMPro;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public static MessageManager main;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI messageText;

    private Coroutine messageCoroutine;
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

    public void ShowMessage(string message, float duration = 2f)
    {
        if (messageText == null)
        {
            Debug.LogError("MessageText no asignado");
            return;
        }

        messageId++;
        int currentId = messageId;

        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }

        messageCoroutine = StartCoroutine(ShowRoutine(message, duration, currentId));
    }

    private IEnumerator ShowRoutine(string message, float duration, int id)
    {
        messageText.gameObject.SetActive(true); // solo texto activo
        messageText.text = message;

        yield return new WaitForSecondsRealtime(duration);

        messageCoroutine = null;

        if (id != messageId)
            yield break;

        ClearMessage();
    }

    public void ShowPersistentMessage(string message)
    {
        messageId++;

        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }

        messageText.gameObject.SetActive(true);
        messageText.text = message;
    }

    public void HideMessage()
    {
        ClearMessage();
    }

    private void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
            messageText.gameObject.SetActive(false);
        }
    }
}