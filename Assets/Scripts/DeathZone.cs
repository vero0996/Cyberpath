using UnityEngine;

public class BaseZone : MonoBehaviour
{ 

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            LevelManager player = FindObjectOfType<LevelManager>();
            EnemyAI2D enemy = other.GetComponent<EnemyAI2D>();

            Jugador jugador = FindFirstObjectByType<Jugador>();
            Damage damage = other.GetComponent<Damage>();

            if (jugador != null && jugador.life && enemy != null)
            {
                jugador.Health -= enemy.damageToPlayer;
               player.GastarMoneda(damage.valorMoneda);
            }
            ContadorEnem.Decrement();
            Destroy(other.gameObject);
        }
    }
}