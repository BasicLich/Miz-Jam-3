using MizJam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackRanged_Boss : StateMachineBehaviour
{
    public GameObject projectilePrefab;
    private Animator animator;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;

        ThrowProjectileAtPlayer(animator.transform);
    }

    private void ThrowProjectileAtPlayer(Transform transform)
    {
        Vector3 dir = (Camera.main.transform.position - transform.position).normalized;
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.position = transform.position;
        //projectile.GetComponent<Projectile>().SetSource(this);
        projectile.GetComponent<Rigidbody>().AddForceAtPosition(dir * 3000, Vector3.zero);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(animator.GetBool("isEnraged"))
            ThrowProjectileAtPlayer(animator.transform);
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
