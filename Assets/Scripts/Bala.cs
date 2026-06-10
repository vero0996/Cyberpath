using UnityEngine;

public class Bala : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Atributos")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private int dmg;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    private void FixedUpdate()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * velocidad;
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null) return;

        // Llamar al componente Damage del enemigo si existe
        if (other.TryGetComponent<Damage>(out var damageComp))
        {
            damageComp.RecibeDano(dmg);
            Destroy(gameObject);
            return;
        }
    }
}