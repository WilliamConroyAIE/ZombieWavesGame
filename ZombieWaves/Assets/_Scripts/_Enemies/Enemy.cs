using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100; 
    private Animator animator;
    private float deathDelay;
    private NavMeshAgent navAgent;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
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

    private void Update()
    {
        if (navAgent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 18f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 21f);
    }
}
