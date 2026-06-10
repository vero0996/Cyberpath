using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Jugador : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    public int maxHealth = 100;
    public int Health;
    public Slider BarraE;
    public Animator animator;
    public bool life = true;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Health = maxHealth;
        BarraE.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        BarraE.value = Health;
        if (life)
        {


            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Movimiento isom�trico
            movement = new Vector2(horizontal, vertical).normalized;
            
            if (Health <= 0 && life)
            {
                Health = 0;
                life = false;
                Invoke("FinJuego", 3);
                
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * velocidad * Time.fixedDeltaTime);
    }

   void FinJuego()
    {
        Debug.Log("FIN JUEGO");

        if (Timer.main != null)
            PlayerData.SetTiempoJugado(Timer.main.GetTiempo());

        APIManager api = FindObjectOfType<APIManager>();

        // Si no existe, lo creamos en el momento
        if (api == null)
        {
            GameObject apiObj = new GameObject("APIManager");
            api = apiObj.AddComponent<APIManager>();
        }

        api.SendKPI(
            Mathf.RoundToInt(Timer.main.GetTiempo()),
            PlayerData.EnemigosMatados,  
            0,   
            0    
        );

        GuardarJuego guardarJuego = FindObjectOfType<GuardarJuego>();
        Time.timeScale = 1f;
        guardarJuego.ClearSavedData();
        SceneManager.LoadScene("GameOver");
    }
}
