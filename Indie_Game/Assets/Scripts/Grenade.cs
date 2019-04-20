using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    public float delay = 3f;
    public float radius = 5f;
    public float force = 700f;
    public float damage = 100f;

    public GameObject explosionEffect;

    float countdown;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        Destroy(gameObject);

        // Show effect
        Destroy(Instantiate(explosionEffect, transform.position, transform.rotation), 1f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearByObject in colliders)
        {
            Rigidbody rb = nearByObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }

            EnemyBehaviourScript enemy = nearByObject.GetComponent<EnemyBehaviourScript>();

            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}
