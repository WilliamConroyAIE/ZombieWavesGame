using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int HP = 100;

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            print("Player Dead");
        }
        else
        {
            print("Player Hit");
            print("Player Health = " + HP);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyHand"))
        {
            TakeDamage(other.GetComponent<ZombieHand>().damage);
        }
    }
}
