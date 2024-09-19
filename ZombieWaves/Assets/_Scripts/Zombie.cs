using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private int HP = 100; 
    private Animator animator;
    private float deathDelay;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0)
        {
            animator.SetTrigger("DIE");
            Invoke("DeathRemover", deathDelay);
        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }
    }

    private void DeathRemover()
    {
        Destroy(gameObject);
    }
}
