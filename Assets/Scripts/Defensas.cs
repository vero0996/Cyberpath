using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Defensas : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private Transform PuntoDeGiro;//punto de giro para la torre
    [SerializeField] private Transform firePoint;//punto de disparo
    [SerializeField] private LayerMask mascaraEnemigo;//capa de enemigos

    [Header("Atributos")]// Atributos relacionados con la salud y el ataque de la defensa
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private float rangoAtaque;
    [SerializeField] private float fireRate=1f;//balas por segundo
    [SerializeField] private float velocidadGiro;

    [Header("Desgaste")]// Atributos relacionados con el desgaste de la defensa
    [SerializeField] private int danoPorSegundo = 1;
    private float timerDesgaste;

    private Transform target;// Objetivo actual al que la defensa está atacando
    private float fireCountdown = Mathf.Infinity;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        timerDesgaste += Time.deltaTime;// Incrementar el timer de desgaste cada frame

        if (timerDesgaste >= 1f)
        {
            TakeDamage(danoPorSegundo);
            timerDesgaste = 0f;
        }
        if (target == null)
        {
            EncuentraObjetivo();// Buscar un nuevo objetivo si no hay ninguno
            return;
        }
        RotarHaciaEnemigo();

        if (!CheckTargetEnRango())
        {
            target = null;
        }
        else
        {
            // Si el objetivo está en rango, incrementar el contador de tiempo para disparar
            fireCountdown += Time.deltaTime;
            if(fireCountdown >= 1f/ fireRate)
            {
                Disparar();
                fireCountdown = 0f;
            }
        }
    }

    private void Disparar()
    {
        // Instanciar la bala en el punto de disparo y configurar su objetivo
        GameObject balaObj = Instantiate(balaPrefab, firePoint.position, Quaternion.identity);
        Bala balaScript = balaObj.GetComponent<Bala>();
        balaScript.SetTarget(target);   
    }
    private void EncuentraObjetivo()
    {
        // Encontrar todos los enemigos dentro del rango de ataque
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rangoAtaque,(Vector2) 
            transform.position, 0f, mascaraEnemigo);

        // Si no se encuentran enemigos, salir del método
        float distanciaMinima = Mathf.Infinity;
        Transform enemigoMasCercano = null;

        foreach (RaycastHit2D hit in hits)
        {
            // Calcular la distancia entre la defensa y el enemigo detectado
            float distancia = Vector2.Distance(
                transform.position,
                hit.transform.position
            );

            if (distancia < distanciaMinima)
            {
                distanciaMinima = distancia;
                enemigoMasCercano = hit.transform;
            }
        }
        // Asignar el enemigo más cercano como objetivo
        target = enemigoMasCercano;
    }
    private bool CheckTargetEnRango()
    {
        // Verificar si el objetivo actual sigue estando dentro del rango de ataque
        if (target == null) return false;
        float distancia = Vector2.Distance(transform.position, target.position);
        return distancia <= rangoAtaque;
    }
    private void RotarHaciaEnemigo()
    {
        // Calcular el ángulo hacia el objetivo y rotar suavemente hacia él
        float angulo = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - 
            transform.position.x) * Mathf.Rad2Deg -180f;
        Quaternion GiroEnemigo = Quaternion.Euler(new Vector3(0f, 0f, angulo));
        PuntoDeGiro.rotation= Quaternion.RotateTowards(PuntoDeGiro.rotation, GiroEnemigo, velocidadGiro * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar un círculo que representa el rango de ataque de la defensa cuando está seleccionada en el editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
    public void TakeDamage(int damage)
    {
        // Reducir la salud actual de la defensa en función del daño recibido
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Destruir la defensa cuando su salud llegue a cero 
        Destroy(gameObject);
    }
}