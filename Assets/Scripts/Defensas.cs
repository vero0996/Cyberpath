using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingHealth : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject balaPrefab;
    [SerializeField] private Transform firePoint;//punto de disparo
    [SerializeField] private LayerMask mascaraEnemigo;//capa de enemigos

    [Header("Atributos")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private float rangoAtaque;
    [SerializeField] private float fireRate=1f;//balas por segundo

    private Transform target;
    private float fireCountdown = Mathf.Infinity;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (target == null)
        {
            EncuentraObjetivo();
            return;
        }

        if (!CheckTargetEnRango())
        {
            target = null;
        }
        else
        { 
           
            
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
        GameObject balaObj = Instantiate(balaPrefab, firePoint.position, Quaternion.identity);
        Bala balaScript = balaObj.GetComponent<Bala>();
        balaScript.SetTarget(target);   
    }
    private void EncuentraObjetivo()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rangoAtaque,(Vector2) 
            transform.position, 0f, mascaraEnemigo);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
    private bool CheckTargetEnRango()
    {
        if (target == null) return false;
        float distancia = Vector2.Distance(transform.position, target.position);
        return distancia <= rangoAtaque;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}