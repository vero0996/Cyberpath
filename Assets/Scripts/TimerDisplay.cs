using UnityEngine;
using TMPro;

public class TimerDisplay : MonoBehaviour
{
    private void Start()
    {
        if (Timer.main != null)
        {
            Timer.main.SetTimerText(
                GetComponent<TextMeshProUGUI>()
            );
        }
    }
}