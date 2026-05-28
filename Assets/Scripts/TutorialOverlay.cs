using UnityEngine;

public class TutorialOverlay : MonoBehaviour
{

    [SerializeField] private GameObject tutorialOverlay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        tutorialOverlay.SetActive(true);
    }

    public void CloseOverlay()
    {
        tutorialOverlay.SetActive(false);
    }
}
