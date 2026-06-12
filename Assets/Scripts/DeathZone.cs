using UnityEngine;

public class BaseZone : MonoBehaviour
{
    // Zona de muerte para el jugador, si el enemigo llega a esta zona,
    // el jugador pierde vida y el enemigo es destruido

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Encontrar el LevelManager para gastar monedas al morir el enemigo
            LevelManager player = FindObjectOfType<LevelManager>();
            EnemyAI2D enemy = other.GetComponent<EnemyAI2D>();

            // Restarle vida al jugador 
            Jugador jugador = FindFirstObjectByType<Jugador>();
            Damage damage = other.GetComponent<Damage>();
            
            if (jugador != null && jugador.life && enemy != null)
            {
                // Aplicar el daño al jugador y restar las monedas correspondientes
                jugador.Health -= enemy.damageToPlayer;
               player.GastarMoneda(damage.valorMoneda);
            }
            ContadorEnem.Decrement();
            Destroy(other.gameObject);
        }
    }
}