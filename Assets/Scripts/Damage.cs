using UnityEngine;

public class Damage : MonoBehaviour
{
    public int herida;
    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.tag == "Jugador" && player.GetComponent<Jugador>().life)
        {
            player.GetComponent<Jugador>().Health -= herida;
            
        }
    }

    void OnCollisionStay2D(Collision2D player)
    {
        if (player.gameObject.tag == "Jugador" &&
            player.gameObject.GetComponent<Jugador>().life)
        {
            player.gameObject.GetComponent<Jugador>().Health -= herida;
           
        }
    }
}