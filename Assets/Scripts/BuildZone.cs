using UnityEngine;
using System.Collections;

public class BuildZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color ColorIntercaccion;
    private GameObject defensa;
    private bool jugadorCerca;

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
            if (Input.GetKeyDown(KeyCode.E))
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

