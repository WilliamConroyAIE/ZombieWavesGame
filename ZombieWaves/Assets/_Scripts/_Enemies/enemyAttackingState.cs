using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAttackingState : StateMachineBehaviour
{
    Transform player;
    NavMeshAgent agent;
    internal bool isCurrentlyAttacking;

    public float stopAttackingDistance = 2.5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       agent = animator.GetComponent<NavMeshAgent>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (SoundManager.Instance.enemyChannel.isPlaying == false)
      {
         SoundManager.Instance.enemyChannel.PlayOneShot(SoundManager.Instance.enemyAttack);
      }
       
       LookAtPlayer();

       float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

       if (distanceFromPlayer > stopAttackingDistance)
       {
            isCurrentlyAttacking = true;
            animator.SetBool("isAttacking", false);    
       }

        Distance(distanceFromPlayer);
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }


    public float currentDistanceFromPlayer;
    private void Distance(float distance)
    {
        currentDistanceFromPlayer = distance;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.enemyChannel.Stop();
    }
}
