using System;
using UnityEngine;
using UnityEngine.UI;

public class gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;
    public float impactForce = 30f;

    public bool burstFire = false;
    public bool singleFire = false;

    public int burstAmount = 3;
    public int ammo = 30;
    public float reloadTime = 2f;

    public Camera fpsCam;
    public ParticleSystem muzzleflash;
    public AudioSource gunSound;

    public GameObject impactEffect;
    public Text ammoText;

    private float nextTimeToFire = 0f;
    private int bulletAmount = 1;
    private int remainingAmmo = 0;
    private float reloadTimer = 0f;
    private bool reloading = false;

    void Start()
    {
        remainingAmmo = ammo;
        reloadTimer = reloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        ammoText.text = string.Format("Ammo: {0} / {1}", remainingAmmo, ammo);
        if (remainingAmmo > 0 && !reloading)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && !singleFire && remainingAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
            else if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && remainingAmmo > 0)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }
        }
        else if(remainingAmmo <= 0)
        {
            reloading = true;
            Reload();
        }
    }

    void Reload()
    {
        if (reloadTimer > 0f)
        {
            reloadTimer = reloadTimer - Time.deltaTime;
        }
        else
        {
            remainingAmmo = ammo;
            reloadTimer = reloadTime;
            reloading = false;
        }
    }

    void Shoot()
    {
        muzzleflash.Play();
        gunSound.Play();

        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            print(hit.transform.name);

            EnemyBehaviourScript enemy = hit.transform.GetComponent<EnemyBehaviourScript>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 3f);
        }
        print(remainingAmmo);
        remainingAmmo -= 1;
    }
}
