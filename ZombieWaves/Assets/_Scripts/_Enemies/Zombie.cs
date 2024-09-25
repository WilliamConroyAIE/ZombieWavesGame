using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieHand zombieHand;
    public Enemy enemy;

    public int zombieDamage;

    private void Start()
    {
        zombieHand.damage = zombieDamage;
    }

    private void Update()
    {
        if (enemy.isDead == true)
        {
            zombieHand.damage = 0;
        }
    }
}
