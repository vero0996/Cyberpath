using UnityEngine;

public class TutorialOverlay : MonoBehaviour
{

    [SerializeField] private GameObject tutorialOverlay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        tutorialOverlay.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void ContinueGame()
    {
        tutorialOverlay.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
}
