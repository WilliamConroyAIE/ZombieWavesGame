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

    public enum ThrowableType
    {
        None,
        FragmentationGrenade/*/,
        ImpactGrenade,
        SmokeGrenade,
        Flashbang/*/
    }
    public ThrowableType throwableType;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
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
        }
    }

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
        }
    }
}
