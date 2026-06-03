using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] private int Daño;
    [SerializeField] private float Cooldown = 1f;
    
 
    private float timerDefensa;

    void OnCollisionStay2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        // DAÑO AL JUGADOR   
        if (obj.CompareTag("Jugador"))
        {
            Jugador jugador = obj.GetComponent<Jugador>();

            if (jugador != null && jugador.life)
            {

                    EnemyAI2D stats = GetComponent<EnemyAI2D>();

                    int damage = (stats != null) ? stats.damageToPlayer : 10;

                    jugador.Health -= damage;

            }
        }

        // DAÑO A DEFENSAS
        if (obj.CompareTag("Defensa"))
        {
            Defensas building = obj.GetComponent<Defensas>();

            if (building != null)
            {
                // si es el primer contacto, golpea instant�neo
                if (timerDefensa == 0f)
                {
                    building.TakeDamage(Daño);
                }

                timerDefensa += Time.deltaTime;

                if (timerDefensa >= Cooldown)
                {
                    building.TakeDamage(Daño);
                    timerDefensa = 0f;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    { 
        timerDefensa = 0f;
    }
    // Daño a enemigos
    [Header("Vida")]
    [SerializeField] private int hitPoints;
    [SerializeField] public int valorMoneda;
    [SerializeField] public int valorPuntos;

    private bool isDead = false;
    public void RecibeDaño(int dmg)
    {
        hitPoints -= dmg;

        if( hitPoints <= 0 && !isDead)
        {
            ContadorEnem.Decrement();
            PlayerData.RegistrarEnemigoMatado();
            LevelManager.main.AddMoneda(valorMoneda);
            LevelManager.main.AddPuntos(valorPuntos);
            isDead = true;
            Destroy(gameObject);
        }
    }
}