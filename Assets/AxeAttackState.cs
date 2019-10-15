using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeAttackState : StateMachineBehaviour
{
    public float attackTime = 0.7f;
    private bool doAttack = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        doAttack = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (doAttack)
            return;

        if (stateInfo.normalizedTime >= attackTime)
        {
            WeaponCtrl.Instance.DamagedByAxe();

            doAttack = true;
        }
    }
}
