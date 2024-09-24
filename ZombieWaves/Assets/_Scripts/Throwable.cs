using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countdown;

    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public bool isImpactThrowable;

    public enum ThrowableType
    {
        None,
        FragmentationGrenade,
        //Pipebomb,
        //MolotovCocktail,
        //Decoy,
        SmokeGrenade
        //Flashbang
    }
    public ThrowableType throwableType;

    private void Start()
    {
        countdown = delay;
        GetComponent<Outline>().enabled = false;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            if (isImpactThrowable) 
            {   
                //Explode on Impact
            }
            else
            {
                countdown -= Time.deltaTime;
                if (countdown <= 0f && !hasExploded)
                {
                    Explode();
                    hasExploded = true;
                }
            }
        }
    }

    private void Explode()
    {
        GetThrowableEffect(throwableType);

        Destroy(gameObject);
    }

    private void GetThrowableEffect(ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case ThrowableType.FragmentationGrenade:
                GrenadeExplosionEffect();
                break;
            /*/case ThrowableType.Pipebomb:
                PipebombEffect();
                break;
            case ThrowableType.MolotovCocktail:
                FireSpreadEffect();
                break;
            case ThrowableType.Decoy:
                break; /*/
            case ThrowableType.SmokeGrenade:
                SmokeBombEffect();
                break;
            //case ThrowableType.Flashbang:
                //FlashBangEffect();
                //break;

            default:
                break;
        }
    }

#region LethalEffects
    private void GrenadeExplosionEffect()
    {
        GameObject explosionEffect = GlobalReference.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.fragmentationGrenadeSoundClip);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }

            if (objectInRange.gameObject.GetComponent<Enemy>())
            {
                objectInRange.gameObject.GetComponent<Enemy>().TakeDamage(100);
            }
        }
    }

    /*
    private void PipebombEffect()
    {
        GameObject explosionEffect = GlobalReference.Instance.PBEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.fragmentationGrenadeSoundClip);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
        }
    }

    private void FireSpreadEffect()
    {
        GameObject explosionEffect = GlobalReference.Instance.MCEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.fragmentationGrenadeSoundClip);

        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
        }
    }
    */
#endregion

#region TacticalEffects
    private void SmokeBombEffect()
    {
        GameObject explosionEffect = GlobalReference.Instance.SmokeExplosionEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.fragmentationGrenadeSoundClip);
    }

    /*
    private void FlashBangEffect()
    {
        GameObject explosionEffect = GlobalReference.Instance.BlindnessEffect;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        SoundManager.Instance.throwablesChannel.PlayOneShot(SoundManager.Instance.fragmentationGrenadeSoundClip);
    }
    */
#endregion
}
