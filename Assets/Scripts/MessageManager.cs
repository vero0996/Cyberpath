using UnityEngine;
using TMPro;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public static MessageManager main;

    [Header("UI")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TextMeshProUGUI messageText;

    private Coroutine messageCoroutine;
    private bool showingPersistentMessage;

    private void Awake()
    {
        main = this;

        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        if (messagePanel == null || messageText == null) return;
        showingPersistentMessage = false;

        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        messageCoroutine = StartCoroutine(ShowMessageRoutine(message, duration));
    }

    private IEnumerator ShowMessageRoutine(string message, float duration)
    {
        messageText.text = message;
        messagePanel.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (!showingPersistentMessage)
        {
            HideMessage();
        }

        messageCoroutine = null;
    }

    public void ShowPersistentMessage(string message)
    {
        if (messagePanel == null || messageText == null) return;

        showingPersistentMessage = true;

        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
            messageCoroutine = null;
        }

        messageText.text = message;
        messagePanel.SetActive(true);
    }

    public void HideMessage()
    {
        if (messagePanel == null || messageText == null) return;

        showingPersistentMessage = false;
        messageText.text = "";
        messagePanel.SetActive(false);
    }
}