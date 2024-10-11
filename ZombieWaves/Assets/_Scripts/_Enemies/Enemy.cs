using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static SoundManager;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100; 
    private Animator animator;
    private float deathDelay;
    private NavMeshAgent navAgent;
    public GameObject enemyHand, AliveEnemyGO, EnemyRagdollGO;

    public bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        isDead = false;
        AliveEnemyGO.SetActive(true);
        EnemyRagdollGO.SetActive(false);
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if (HP <= 0 && !isDead)
        {
            //animator.SetBool("isDEAD", true);
            isDead = true;

            AliveEnemyGO.SetActive(false);
            EnemyRagdollGO.SetActive(true);

            SoundManager.Instance.enemyChannel2.PlayOneShot(SoundManager.Instance.enemyDeath);

            enemyHand.SetActive(false);
        }
        else
        {
            animator.SetTrigger("DAMAGE");
            SoundManager.Instance.enemyChannel2.PlayOneShot(SoundManager.Instance.enemyHurt);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 20f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 25f);
    }
}
