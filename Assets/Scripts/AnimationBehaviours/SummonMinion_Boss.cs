using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMinion_Boss : StateMachineBehaviour
{
    public GameObject[] minionsPrefabs;
    public GameObject summonFXPrefab;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int rand = Random.Range(0, minionsPrefabs.Length);
        GameObject minion = Instantiate(minionsPrefabs[rand]);
        
        Vector3 summonPos = animator.transform.position + animator.transform.Find("Body").forward * 6;
        minion.transform.position = summonPos;
        Instantiate(summonFXPrefab).transform.position = summonPos;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
