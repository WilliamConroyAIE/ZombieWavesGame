using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + ".");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().isDead == false)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }
            CreateHitEffect(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("EnemyHead"))
        {
            if (collision.gameObject.GetComponent<Enemy>().isDead == false)
            {
                collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(bulletDamage * 2);
            }
            CreateHitEffect(collision);
            Destroy(gameObject);
        }
        
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject hole = Instantiate(GlobalReference.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }

    void CreateHitEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject effect = Instantiate(GlobalReference.Instance.hitEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        effect.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
