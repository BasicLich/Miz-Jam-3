using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle_Boss : StateMachineBehaviour
{
    public float runRange;
    private Vector3 playerFloor;
    private NavMeshAgent navMeshAgent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdatePlayerFloorPos(animator.transform.position.y);

        //KeepDistanceFromPlayer
        if ((playerFloor - animator.transform.position).magnitude < runRange)
        {
            Vector3 dir = (animator.transform.position - Camera.main.transform.position).normalized;
            dir.y = animator.transform.position.y;
            navMeshAgent.destination = animator.transform.position + dir;
        }
    }

    private void UpdatePlayerFloorPos(float thisY)
    {
        playerFloor = Camera.main.transform.position;
        playerFloor.y = thisY;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
