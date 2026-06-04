using UnityEngine;

public class WebGLBridge : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetUserId(string userId)
    {
        int id = int.Parse(userId);

        PlayerPrefs.SetInt("userId", id);
        PlayerPrefs.Save();

        Debug.Log("USER RECIBIDO DESDE REACT: " + id);
    }
}