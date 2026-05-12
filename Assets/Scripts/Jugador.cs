using UnityEngine;
using UnityEngine.UI;

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
    }

    // Update is called once per frame
    void Update()
    {
        BarraE.value = Health;
        if (life)
        {


            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Movimiento isométrico
            movement = new Vector2(horizontal, vertical).normalized;
            // Animaciones
            if (animator != null)
            {
                animator.SetFloat("MoveX", movement.x);
                animator.SetFloat("MoveY", movement.y);
                animator.SetFloat("Speed", movement.sqrMagnitude);
            }
            if (Health <= 0 && life)
            {
                Health = 0;
                life = false;
                animator.Play("PlayerDead");

            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * velocidad * Time.fixedDeltaTime);
    }


}
