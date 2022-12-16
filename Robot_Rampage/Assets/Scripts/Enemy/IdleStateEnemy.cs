using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateEnemy : StateMachineBehaviour
{
    float timer;
    // Player used to determine position
    Transform player;

    // Chase and attack ranges set based on the value in the specific enemy
    float chaseRange;
    float attackRange;

    // Ability to chase or attack based on the value in the specific enemy
    bool canChase;
    bool canAttack;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        chaseRange = animator.transform.GetComponent<EnemyAction>().chaseRange;
        attackRange = animator.transform.GetComponent<EnemyAction>().attackRange;

        canChase = animator.transform.GetComponent<EnemyAction>().canChase;
        canAttack = animator.transform.GetComponent<EnemyAction>().canAttack;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
/*        timer += Time.deltaTime;
        if (timer > 5)
        {
            animator.SetBool("isPatrolling", true);
        }*/

        float playerDistance = Vector3.Distance(player.position, animator.transform.position);

        if (playerDistance < attackRange && canAttack)
        {
            animator.SetBool("isAttacking", true);
        }
        else if (playerDistance < chaseRange && canChase)
        {
            animator.SetBool("isChasing", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
