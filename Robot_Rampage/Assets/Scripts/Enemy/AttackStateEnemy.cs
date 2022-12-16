using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateEnemy : StateMachineBehaviour
{

    // Player used to determine position
    Transform player;

    // Attack range set based on the value in the specific enemy
    float attackRange;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        attackRange = animator.transform.GetComponent<EnemyAction>().attackRange;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        // Find player position and properly rotate the enemy towards the player
        Vector3 playerPosition = new Vector3(player.position.x, animator.transform.position.y, player.position.z);
        animator.transform.LookAt(playerPosition);
        animator.transform.rotation *= Quaternion.Euler(0, 90, 0);

        float playerDistance = Vector3.Distance(player.position, animator.transform.position);

        if (playerDistance > attackRange)
        {
            animator.SetBool("isAttacking", false);
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
