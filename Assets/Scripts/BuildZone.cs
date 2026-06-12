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
    private Color startColor; // Color original del sprite

    private void Start()
    {
        // Guardar el color inicial para restaurarlo después
        startColor = sr.color;
    }

    private void Update()
    {
        // Ejecutar lógica únicamente si el jugador está cerca
        if (jugadorCerca)
        {
            // Cambiar el color para indicar interacción
            sr.color = ColorIntercaccion;

            // Presionar E para construir una defensa
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Si ya existe una defensa en esta zona, salir
                if (defensa != null) return;

                // Obtener la defensa seleccionada actualmente
                Torres defensatemp = BuildManager.main.GetSelectedDefensa();

                // Si no hay ninguna defensa seleccionada
                if (defensatemp == null)
                {
                    if (MessageManager.main != null)
                    {
                        MessageManager.main.ShowMessage("No defense selected!");
                    }

                    return;
                }

                // Verificar si hay dinero suficiente
                if (defensatemp.precio > LevelManager.main.moneda)
                {
                    Debug.Log("No tienes suficiente dinero");

                    if (MessageManager.main != null)
                    {
                        MessageManager.main.ShowMessage("Not enough money!");
                    }

                    return;
                }

                // Descontar el costo de la defensa
                LevelManager.main.GastarMoneda(defensatemp.precio);

                // Crear la defensa en la posición de la zona
                defensa = Instantiate(
                    defensatemp.prefab,
                    transform.position,
                    Quaternion.identity
                );

                Debug.Log("Interactuo " + nameof(BuildZone));

                // Aumentar contador de defensas construidas
                defensaConstruida += 1;

                // Registrar estadística de defensa utilizada
                PlayerData.RegistrarDefensaUsada();

                // Mostrar mensaje de confirmación
                if (MessageManager.main != null)
                {
                    MessageManager.main.ShowMessage("Defense placed!");
                }
            }

            // Presionar Q para vender la defensa
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Verificar que exista una defensa para vender
                if (defensa == null)
                {
                    Debug.Log("No hay defensa para vender");

                    if (MessageManager.main != null)
                    {
                        MessageManager.main.ShowMessage("There is no defense to sell!");
                    }

                    return;
                }

                // Evitar vender la única defensa disponible
                if (defensa != null &&
                    defensaConstruida < 2 &&
                    LevelManager.main.moneda < 70)
                {
                    Debug.Log("Unica defensa en el campo, no se puede vender");

                    if (MessageManager.main != null)
                    {
                        MessageManager.main.ShowMessage("Cannot sell the only defense!");
                    }

                    return;
                }

                // Obtener el precio original de la defensa
                int precio = BuildManager.main.GetPrice(defensa);
                int reembolso = Mathf.FloorToInt(precio * 0.5f); // Reembolsar el 50% del precio original

                LevelManager.main.AddMoneda(reembolso); // Agregar las monedas correspondientes
                Destroy(defensa);

                // Eliminar referencia a la defensa
                defensa = null;
                defensaConstruida -= 1;// Reducir contador de defensas construidas

                Debug.Log("Destruyo " + nameof(BuildZone));

                // Mostrar mensaje de confirmación
                if (MessageManager.main != null)
                {
                    MessageManager.main.ShowMessage("Defense sold!");
                }
            }
        }
        else
        {
            // Restaurar el color original cuando el jugador se aleja
            sr.color = startColor;
        }
    }

    // Detectar cuando el jugador entra a la zona
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            // Activar la interacción
            jugadorCerca = true;

            // Mostrar instrucciones en pantalla
            if (MessageManager.main != null)
            {
                MessageManager.main.ShowMessage(
                    "Press E to place a defense, Q to sell it"
                );
            }
        }
    }

    // Detectar cuando el jugador sale de la zona
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Jugador"))
        {
            // Desactivar la interacción
            jugadorCerca = false;

            // Ocultar mensaje de ayuda
            if (MessageManager.main != null)
            {
                MessageManager.main.HideMessage();
            }
        }
    }
}