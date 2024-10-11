using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using TMPro;
using static HUDManager;
using static WeaponManager;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public Animator animator;
    public int weaponDamage;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30f, bulletPrefabLifeTime = 3f, shootingDelay = 2f, lerpSpeed = 15f; 
    public float spreadIntensity, hipSpreadIntensity, adsSpreadIntensity;

    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public int bulletsPerBurst = 1, burstBulletsLeft;
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
        MicroUzi,
        M1Garand,
        MG42,
        SA80,
        Famas,
        MP5,
        MP40,
        AK47,
        AK74U,
        Dragunov,
        PPSH,
        Luger,
        DesertEagle,
        M9Beretta,
        Revolver,
        Glock17,
        Tokarev,
        Makarov,
        M1911
    }
    public WeaponModel thisWeaponModel;

    [Header("Reloading")]
    public float reloadTime;
    public int magazineSize, bulletsLeft;
    public bool isReloading;

    [Header("ADS")]

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;
    public Vector3 ADSPosition;
    public Vector3 ADSRotation;
    bool isADS;

    public GameObject Vertical, Horizontal; 

    [Header("FireMode")]
    public bool Switchable;
    public bool BurstAllowed;

    public Camera wrCam;
    public Camera srCam = null;
    public bool isScoped;

    private void Awake()
    {
       readyToShoot = true;
       burstBulletsLeft = bulletsPerBurst; 
       animator = GetComponent<Animator>();
       bulletsLeft = magazineSize;
       GetComponent<Outline>().enabled = false;

       if (thisWeaponModel == WeaponModel.Dragunov || thisWeaponModel == WeaponModel.M1Garand)
       {
            isScoped = true;
       }
       else
       {
            isScoped = false;
       }

        animator.SetBool("isADS", false);
        HUDManager.Instance.crosshair.SetActive(true);
        spreadIntensity = hipSpreadIntensity;
        Camera.main.GetComponent<MouseLook>().ChangeToHipSensitivity();

        if (isScoped)
        {
            wrCam.enabled = true;
            srCam.enabled = false;
            Vertical.SetActive(false);
            Horizontal.SetActive(false);
         }
        else
        {
            wrCam.enabled = true;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60.0f, lerpSpeed * Time.deltaTime);

            Vertical.SetActive(false);
            Horizontal.SetActive(false);
        }
    }

    private void Update()
    {
        if (isActiveWeapon)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Weapon");
            }

            GetComponent<Outline>().enabled = false;

            if (Input.GetMouseButtonDown(1) && !isADS)
            {
                enterADS();
            }
            else if (Input.GetMouseButtonDown(1) && isADS)
            {
                exitADS();
            }

            if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);

                if (isShooting)
                {
                    if (currentShootingMode == ShootingMode.Single)
                    {
                        bulletsPerBurst = 1;
                        animator.SetBool("isSingle", true);
                        animator.SetTrigger("Recoil");
                    }
                    else
                    {
                        animator.SetBool("isSingle", false);
                    }

                    if (currentShootingMode == ShootingMode.Burst)
                    {
                        bulletsPerBurst = 3;
                        animator.SetBool("isBurst", true);
                        animator.SetTrigger("Recoil");
                    }
                    else
                    {
                        animator.SetBool("isBurst", false);
                    }
                }
            }
            else
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
                bulletsPerBurst = 1;

                if (isShooting)
                {
                    animator.SetBool("Fire", true);
                    animator.SetBool("isAuto", true);
                }
                else
                {
                    animator.SetBool("Fire", false);
                    animator.SetBool("isAuto", false);
                }   
            }

            if (bulletsLeft == 0 && !isReloading)
            {
                SoundManager.Instance.emptyChannel.Play();
            }

            if (readyToShoot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading && WeaponManager.Instance.CheckAmmunitionLeftFor(thisWeaponModel) > 0)
                Reload();
            
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                Reload();
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            transform.localPosition = transform.localPosition;
            transform.localRotation = transform.localRotation;
        }

        //FireModeSwitching
        FireModeSwitchChecker();

        if (Input.GetKeyDown(KeyCode.V) && Switchable)
        {
            if (BurstAllowed && currentShootingMode == ShootingMode.Single)
            {
                currentShootingMode = ShootingMode.Burst;
            }
            //
            else if (!BurstAllowed && currentShootingMode == ShootingMode.Single)
            {
                currentShootingMode = ShootingMode.Auto;
            }
            //
            else if (BurstAllowed && currentShootingMode == ShootingMode.Burst)
            {
                currentShootingMode = ShootingMode.Auto;
            }
            //
            else if (currentShootingMode == ShootingMode.Auto)
            {
                currentShootingMode = ShootingMode.Single;
            }
            //
            else
            {
                currentShootingMode = currentShootingMode;
            }
        }
        else
        {
            currentShootingMode = currentShootingMode;
        }
        
        //AimDownSights();

        if (isADS && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) || isADS && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W) || isADS && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.S) || isADS && Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D))
        {
            exitADS();
        }
    }

    private void FireModeSwitchChecker()
    {
        if (thisWeaponModel == WeaponModel.M16A4 || thisWeaponModel == WeaponModel.SA80)
        {
            Switchable = true;
            BurstAllowed = true;
        }
        else if (thisWeaponModel == WeaponModel.MicroUzi || thisWeaponModel == WeaponModel.MP5 || thisWeaponModel == WeaponModel.AK47 || thisWeaponModel == WeaponModel.AK74U || thisWeaponModel == WeaponModel.PPSH || thisWeaponModel == WeaponModel.Glock17)
        {
            Switchable = true;
            BurstAllowed = false;
        }
        else
        {
            Switchable = false;
            BurstAllowed = false;
        }
    }

    #region Shooting
    private void FireWeapon()
    {
        bulletsLeft--;
        readyToShoot = false;
        
        muzzleFlash.GetComponent<ParticleSystem>().Play();
        SoundManager.Instance.PlayShootingSound(thisWeaponModel);
        
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //Working PhysicsBasedShooting
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;
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

    private Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
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

    
    #endregion

    #region Reload

    private void Reload()
    {
        exitADS();
        isReloading = true;
        animator.SetTrigger("Reload");
        SoundManager.Instance.PlayReloadingSound();
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        if (WeaponManager.Instance.CheckAmmunitionLeftFor(thisWeaponModel) > magazineSize)
        {
            bulletsLeft = magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmunition(bulletsLeft, thisWeaponModel);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmunitionLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmunition(bulletsLeft, thisWeaponModel);
        }
        isReloading = false;
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

    #endregion


    #region ADS
    /*private void AimDownSights()
    {
        if (Input.GetMouseButton(1) & !isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, ADSPosition, Time.deltaTime * lerpSpeed);
            transform.localRotation = Quaternion.Euler(0f, 1.5f, 0f);

            HUDManager.Instance.crosshair.SetActive(false);
            spreadIntensity = adsSpreadIntensity;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 40.0f, lerpSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, spawnPosition, Time.deltaTime * lerpSpeed);
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

            HUDManager.Instance.crosshair.SetActive(true);
            spreadIntensity = hipSpreadIntensity;
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60.0f, lerpSpeed * Time.deltaTime);
        }
    }*/

    
    //Broken ADS
    
    private void enterADS()
    {
        isADS = true;
        
        if (isADS)
        {
            animator.SetBool("isADS", true);
            HUDManager.Instance.crosshair.SetActive(false);
            spreadIntensity = adsSpreadIntensity;
            Camera.main.GetComponent<MouseLook>().ChangeToADSSensitivity();

            if (isScoped)
            {
                wrCam.enabled = false;
                srCam.enabled = true;
                Vertical.SetActive(true);
                Horizontal.SetActive(true);
            }
            else
            {
                wrCam.enabled = true;
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 40.0f, lerpSpeed * Time.deltaTime);
                Vertical.SetActive(false);
                Horizontal.SetActive(false);
            }
        }
    }

    private void exitADS()
    {
        isADS = false;
        
        if (!isADS)
        {
            animator.SetBool("isADS", false);
            HUDManager.Instance.crosshair.SetActive(true);
            spreadIntensity = hipSpreadIntensity;
            Camera.main.GetComponent<MouseLook>().ChangeToHipSensitivity();

            if (isScoped)
            {
                wrCam.enabled = true;
                srCam.enabled = false;
                Vertical.SetActive(false);
                Horizontal.SetActive(false);
            }
            else
            {
                wrCam.enabled = true;
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60.0f, lerpSpeed * Time.deltaTime);

                Vertical.SetActive(false);
                Horizontal.SetActive(false);
            }
            }
    }
    
    #endregion
}
