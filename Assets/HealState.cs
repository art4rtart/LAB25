using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealState : StateMachineBehaviour
{
    public float healTime = 0.7f;
    private bool isHeal = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isHeal)
            return;
        if (stateInfo.normalizedTime >= healTime)
        {
            //int prevHp = AgentManager.hp;
            //AgentManager.hp += 70;
            //if (AgentManager.hp > 100)
            //    AgentManager.hp = 100;

            //if (AgentManager.hp - prevHp > 50)
            //    Healing.hpGapBetterThan50 = true;
            //else
            //    Healing.hpGapBetterThan50 = false;

            PlayerManager.hp += 70;
            isHeal = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(PlayerManager.hp);
        animator.SetBool("isHeal", false);
        isHeal = false;
        //Healing.hpGapBetterThan50 = false;
    }
}
