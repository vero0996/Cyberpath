using UnityEngine;

public class EnemyAI2D : MonoBehaviour
{
    public Transform[] waypoints;
    private Rigidbody2D rb;
    public float speed = 3f;
    private int currentWaypoint = 0;
    public int damageToPlayer ;
    public float detectionRange = 4f; // Distancia para detectar objetos
    private Transform target; // Objeto de interés

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
       
    }
    void Update()
    {
        // Si tiene objetivo
        if (target != null)
        {
            ChaseTarget();

            // Si el objeto desaparece
            if (!target.gameObject.activeInHierarchy)
            {
                target = null;
                FindClosestWaypoint();
            }
        }
        else
        {
            FollowPath();
            SearchForTarget();
        }
    }

    void FollowPath()
    {
        if (currentWaypoint >= waypoints.Length)
            return;

        Transform wp = waypoints[currentWaypoint];

        rb.position = Vector2.MoveTowards(
            rb.position,
            wp.position,
            speed * Time.deltaTime
        );

        // Llegó al waypoint
        if (Vector2.Distance(rb.position, wp.position) < 0.1f)
        {
            currentWaypoint++;
        }
    }

    void ChaseTarget()
    {
        rb.position = Vector2.MoveTowards(
            rb.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    void SearchForTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            detectionRange
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Defensa"))
            {
                target = hit.transform;
                break;
            }
        }
    }
    public void SetPath(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypoint = 0;
    }


    void FindClosestWaypoint()
    {
        float minDist = Mathf.Infinity;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float dist = Vector2.Distance(
                transform.position,
                waypoints[i].position
            );

            if (dist < minDist)
            {
                minDist = dist;
                currentWaypoint = i;
            }
        }
    }

}