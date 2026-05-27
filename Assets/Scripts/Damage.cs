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

        // DA�O A DEFENSAS
        if (obj.CompareTag("Interest"))
        {
            BuildingHealth building = obj.GetComponent<BuildingHealth>();

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
    // Da�o a enemigos
    [Header("Vida")]
    [SerializeField] private int hitPoints;
    [SerializeField] public int valorMoneda;

    private bool isDead = false;
    public void RecibeDaño(int dmg)
    {
        hitPoints -= dmg;

        if( hitPoints <= 0 && !isDead)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.AddMoneda(valorMoneda);
            isDead = true;
            Destroy(gameObject);
        }
    }
}