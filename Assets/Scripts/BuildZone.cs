using UnityEngine;
using System.Collections;

public class BuildZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color ColorIntercaccion;
    private GameObject defensa;
    private bool jugadorCerca;
    private int defensaConstruida = 0;
    private Color startColor;

    private void Start()
        {
            startColor = sr.color;
    }

    private void Update()
    {
        if (jugadorCerca)
        {
            sr.color = ColorIntercaccion;
            if (Input.GetKeyDown(KeyCode.E)) //Construir defensa
            {
                if (defensa != null) return;
               
                Torres defensatemp = BuildManager.main.GetSelectedDefensa();
                if (defensatemp.precio > LevelManager.main.moneda)
                {
                    Debug.Log("No tienes suficiente dinero");
                    return;
                }
                LevelManager.main.GastarMoneda(defensatemp.precio);

                defensa = Instantiate(defensatemp.prefab, transform.position, Quaternion.identity);
                Debug.Log("Interactuó" + nameof(BuildZone));
                defensaConstruida += 1;

            }
            if (Input.GetKeyDown(KeyCode.Q)) //Destruir defensa
            {
                if (defensa == null)
                {
                    Debug.Log("No hay defensa para vender");
                    return;
                }
                if (defensa != null && defensaConstruida < 2 && LevelManager.main.moneda < 70)
                {
                   Debug.Log("Unica defensa en el campo, no se puede vender");
                    return;
                }

                int precio = BuildManager.main.GetPrice(defensa);
                int reembolso = Mathf.FloorToInt(precio * 0.5f);
                LevelManager.main.AddMoneda(reembolso); //Vender defensa por la mitad de su precio
                Destroy(defensa);   
                defensa = null;
                defensaConstruida -= 1;
                Debug.Log("Destruyó" + nameof(BuildZone));
            }
        }
        else
        {
            sr.color = startColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            jugadorCerca = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            jugadorCerca = false;
        }
    }
}

