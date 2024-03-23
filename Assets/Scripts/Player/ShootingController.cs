using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private GameObject bulletPrefab; // Префаб пули
    [SerializeField] private Transform firePoint;

    private float delayShot = 0.6f;
    private bool isShooting = false;
    private float timeBtwShots;

    void Start()
    {
        ApplyStartTimeBtwShots();
    }

    public void ApplyStartTimeBtwShots()
    {
        delayShot = PlayerPrefs.GetFloat("DelayShot", 0.6f);
        timeBtwShots = delayShot;
    }

    public void ShootingOn()
    {
        isShooting = true;
    }

    public void ShootingOff()
    {
        isShooting = false;
    }

    private void Shoot()
    {
        if (timeBtwShots <= 0)
        {
            if (muzzleFlash != null) muzzleFlash.Play();
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            timeBtwShots = delayShot;
        }
    }
    void FixedUpdate()
    {
        if (timeBtwShots >= 0)
            timeBtwShots -= Time.deltaTime;

        if (isShooting) 
            Shoot();
    }
}
