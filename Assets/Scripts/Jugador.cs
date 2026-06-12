using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Jugador : MonoBehaviour
{
    // Atributos del jugador
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
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        BarraE.value = Health;// Actualizar la barra de salud
        if (life)
        {
            // Obtener input del jugador
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Movimiento isom�trico
            movement = new Vector2(horizontal, vertical).normalized;

            if (Health <= 0 && life)
            {
                Health = 0;
                life = false;

                // Detener movimiento
                movement = Vector2.zero;
                rb.linearVelocity = Vector2.zero;

                // Reproducir animación de muerte
                animator.Play("Jugador_Death");
                
                // Esperar a que termine la animación
                Invoke("FinJuego", 3f);
            }
        }
    }

    void FixedUpdate()// Se llama a intervalos fijos, ideal para física
    {
        if (life)
        {
            rb.MovePosition(rb.position + movement * velocidad * Time.fixedDeltaTime);
        }
    }

    // Método para terminar el juego después de la muerte del jugador, enviando estadísticas al backend y limpiando el estado guardado
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

        // Enviar las estadísticas del juego al backend
        api.SendKPI(
            Mathf.RoundToInt(Timer.main.GetTiempo()),
            PlayerData.EnemigosMatados,  
            0,   
            0    
        );
        // Limpiar el save para evitar que el jugador pueda continuar desde un estado anterior
        GuardarJuego guardarJuego = FindObjectOfType<GuardarJuego>();
        Time.timeScale = 1f;
        guardarJuego.ClearSavedData();
        SceneManager.LoadScene("GameOver");
    }
}
