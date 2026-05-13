using UnityEngine;

public class Damage : MonoBehaviour
{
    public int herida;
    public float Cooldown = 1f;

 
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
        if (obj.CompareTag("Interest"))
        {
            BuildingHealth building = obj.GetComponent<BuildingHealth>();

            if (building != null)
            {
                // si es el primer contacto, golpea instantáneo
                if (timerDefensa == 0f)
                {
                    building.TakeDamage(herida);
                }

                timerDefensa += Time.deltaTime;

                if (timerDefensa >= Cooldown)
                {
                    building.TakeDamage(herida);
                    timerDefensa = 0f;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    { 
        timerDefensa = 0f;
    }
}