using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public Animator animator;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f, bulletPrefabLifeTime = 3f, shootingDelay = 2f, spreadIntensity;

    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public int bulletsPerBurst = 3, burstBulletsLeft;
    public GameObject muzzleFlash;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }
    public ShootingMode currentShootingMode;

    public enum WeaponModel
    {
        M16A4, 
        M1911
    }
    public WeaponModel thisWeaponModel;

    [Header("Reloading")]
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    [Header("Additional")]
    public Camera playerCamera;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public Vector3 ADSPosition;
    public Vector3 ADSRotation;

    private void Awake()
    {
       readyToShoot = true;
       burstBulletsLeft = bulletsPerBurst; 
       animator = GetComponent<Animator>();
       bulletsLeft = magazineSize;
    }

    private void Update()
    {
        GetComponent<Outline>().enabled = false;

        if (isActiveWeapon)
        {
            if (bulletsLeft == 0 && !isReloading)
            {
                SoundManager.Instance.emptyChannel.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
                Reload();
            
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                Reload();
            }
        }
    }

    private void FireWeapon()
    {
        bulletsLeft--;
        readyToShoot = false;
        animator.SetTrigger("Recoil");
        
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletVelocity / 4, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        isReloading = true;
        animator.SetTrigger("Reload");
        SoundManager.Instance.PlayReloadingSound();
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray= playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
}
