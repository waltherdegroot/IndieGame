using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeSystem : MonoBehaviour
{
    public float throwForce = 10f;
    public float maxThrowForce = 15f;
    public GameObject grenadePrefab;

    private float charge;

    void Start()
    {
        charge = throwForce;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && PlayerManager.instance.grenades > 0)
        {
            ThrowGrenade();
            PlayerManager.instance.grenades -= 1;
        }
    }

    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
    }
}
