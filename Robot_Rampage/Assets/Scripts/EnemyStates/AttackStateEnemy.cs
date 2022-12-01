using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateEnemy : StateMachineBehaviour
{

    float timer;
    Transform player;

    float attackRange = 20;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*        timer += Time.deltaTime;
                if (timer > 5)
                {
                    animator.SetBool("isPatrolling", true);
                }*/

        Vector3 playerPosition = new Vector3(player.position.x, 0, player.position.z);

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
