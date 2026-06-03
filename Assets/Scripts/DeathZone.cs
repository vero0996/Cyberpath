using UnityEngine;

public class BaseZone : MonoBehaviour
{ 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
           
            EnemyAI2D enemy = other.GetComponent<EnemyAI2D>();

            Jugador jugador = FindFirstObjectByType<Jugador>();

            if (jugador != null && jugador.life && enemy != null)
            {
                jugador.Health -= enemy.damageToPlayer;
            }
            ContadorEnem.Decrement();
            Destroy(other.gameObject);
        }
    }
}