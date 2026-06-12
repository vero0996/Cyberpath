using UnityEngine;

public class EnemyAI2D : MonoBehaviour
{
    // Atributos del enemigo
    public Transform[] waypoints;
    private Rigidbody2D rb;
    public float speed = 3f;
    private int currentWaypoint = 0;
    public int damageToPlayer;
    public float detectionRange = 4f;
    private Transform target;

    void Start()
    {
        // Obtener el Rigidbody2D asociado al enemigo
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Si existe un objetivo, perseguirlo
        if (target != null)
        {
            ChaseTarget();

            // Si el objetivo fue destruido o desactivado,
            // volver a seguir el camino
            if (!target.gameObject.activeInHierarchy)
            {
                target = null;
                FindClosestWaypoint();
            }
        }
        else
        {
            // Continuar avanzando por los waypoints
            FollowPath();

            // Buscar defensas cercanas
            SearchForTarget();
        }
    }

    // Hace que el enemigo avance por el camino establecido
    void FollowPath()
    {
        // Si ya llegó al final del recorrido, detenerse
        if (currentWaypoint >= waypoints.Length)
            return;

        // Obtener el waypoint actual
        Transform wp = waypoints[currentWaypoint];

        // Mover al enemigo hacia el waypoint
        rb.position = Vector2.MoveTowards(
            rb.position,
            wp.position,
            speed * Time.deltaTime
        );

        // Si llegó suficientemente cerca, pasar al siguiente waypoint
        if (Vector2.Distance(rb.position, wp.position) < 0.1f)
        {
            currentWaypoint++;
        }
    }

    // Persigue el objetivo actual
    void ChaseTarget()
    {
        rb.position = Vector2.MoveTowards(
            rb.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    // Busca objetos cercanos dentro del rango de detección
    void SearchForTarget()
    {
        // Obtener todos los colliders cercanos
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            detectionRange
        );

        // Recorrer los objetos detectados
        foreach (Collider2D hit in hits)
        {
            // Si encuentra una defensa, convertirla en objetivo
            if (hit.CompareTag("Defensa"))
            {
                target = hit.transform;
                break;
            }
        }
    }

    // Asigna un nuevo camino al enemigo
    public void SetPath(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;

        // Reiniciar desde el primer waypoint
        currentWaypoint = 0;
    }

    // Encuentra el waypoint más cercano a la posición actual
    void FindClosestWaypoint()
    {
        float minDist = Mathf.Infinity;

        // Revisar todos los waypoints
        for (int i = 0; i < waypoints.Length; i++)
        {
            float dist = Vector2.Distance(
                transform.position,
                waypoints[i].position
            );

            // Guardar el waypoint más cercano
            if (dist < minDist)
            {
                minDist = dist;
                currentWaypoint = i;
            }
        }
    }
}