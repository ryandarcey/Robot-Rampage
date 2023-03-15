using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunStateEnemy : StateMachineBehaviour
{
    // Player used to determine position
    Transform player;

    GameObject settingsManager;

    // Chase and attack ranges set based on the value in the specific enemy
    float chaseRange;
    float attackRange;

    // Ability to attack based on the value in the specific enemy
    bool canAttack;

    // Speed of chasing based on the value in the specific enemy
    float chaseSpeed;
    float chaseSpeedMultiplier = 1;

    /*    List<Transform> waypoints = new List<Transform>();
        NavMeshAgent agent;*/
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        chaseRange = animator.transform.GetComponent<EnemyAction>().chaseRange;
        attackRange = animator.transform.GetComponent<EnemyAction>().attackRange;

        canAttack = animator.transform.GetComponent<EnemyAction>().canAttack;

        chaseSpeed = animator.transform.GetComponent<EnemyAction>().chaseSpeed;
        chaseSpeedMultiplier = FindObjectOfType<SettingsManager>().getEnemyMovementSpeed();

        // Play looping walk sound
        FindObjectOfType<AudioManager>().PlaySound("enemy run");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find player position and properly rotate the enemy towards the player
        Vector3 playerPosition = new Vector3(player.position.x, animator.transform.position.y, player.position.z);
        animator.transform.LookAt(playerPosition);
        animator.transform.rotation *= Quaternion.Euler(0, 90, 0);
        //animator.transform.Rotate(new Vector3(0, 90f, 0));
        //animator.transform.position += animator.transform.forward * Time.deltaTime * 8f;
        animator.transform.Translate(Vector3.left * Time.deltaTime * (chaseSpeed * chaseSpeedMultiplier), Space.Self);

        float playerDistance = Vector3.Distance(player.position, animator.transform.position);

        if (playerDistance < attackRange && canAttack)
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

        // Stop looping walk sound
        FindObjectOfType<AudioManager>().StopSound("enemy run");
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
