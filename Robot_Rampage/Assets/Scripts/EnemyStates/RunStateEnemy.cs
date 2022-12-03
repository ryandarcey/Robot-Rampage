using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunStateEnemy : StateMachineBehaviour
{

    float timer;

    Transform player;

    float chaseRange = 25;
    float attackRange = 10;

    /*    List<Transform> waypoints = new List<Transform>();
        NavMeshAgent agent;*/
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        /*        agent = animator.GetComponent<NavMeshAgent>();

                GameObject points = GameObject.FindGameObjectWithTag("EnemyWaypoints");
                foreach (Transform w in points.transform)
                {
                    waypoints.Add(w);
                }

                agent.SetDestination(waypoints[Random.Range(0, waypoints.Count)].position);
        */
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*        if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    agent.SetDestination(waypoints[Random.Range(0, waypoints.Count)].position);
                }*/

        /*        timer += Time.deltaTime;
                if (timer > 5)
                {
                    animator.SetBool("isPatrolling", false);
                }*/

        Vector3 playerPosition = new Vector3(player.position.x, animator.transform.position.y, player.position.z);
        animator.transform.LookAt(playerPosition);
        animator.transform.rotation *= Quaternion.Euler(0, 90, 0);
        //animator.transform.Rotate(new Vector3(0, 90f, 0));
        //animator.transform.position += animator.transform.forward * Time.deltaTime * 8f;
        animator.transform.Translate(Vector3.left * Time.deltaTime * 8f, Space.Self);

        float playerDistance = Vector3.Distance(player.position, animator.transform.position);

        if (playerDistance < attackRange)
        {
            animator.SetBool("isAttacking", true);
        }
        else if (playerDistance > chaseRange)
        {
            animator.SetBool("isChasing", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
/*        agent.SetDestination(agent.transform.position);*/
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
