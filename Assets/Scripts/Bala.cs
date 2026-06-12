using UnityEngine;

public class Bala : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Atributos")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private int dmg;
    private Transform target;

    // Asigna el enemigo u objetivo al que seguirá la bala
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void FixedUpdate()
    {
        // Si el objetivo ya no existe, destruir la bala
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        // Calcular la dirección hacia el objetivo
        Vector2 direction = (target.position - transform.position).normalized;

        // Mover la bala en dirección al objetivo con la velocidad indicada
        rb.linearVelocity = direction * velocidad;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar que el collider recibido no sea nulo
        if (other == null) return;

        // Intentar obtener el componente Damage del objeto impactado
        if (other.TryGetComponent<Damage>(out var damageComp))
        {
            // Aplicar daño al enemigo
            damageComp.RecibeDano(dmg);

            // Destruir la bala después del impacto
            Destroy(gameObject);
            return;
        }
    }
}