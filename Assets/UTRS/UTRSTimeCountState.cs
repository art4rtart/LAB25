using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UTRSTimeCountState : StateMachineBehaviour
{
	bool temp = false;
	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		UTRSManager.Instance.leftTime =UTRSManager.Instance.waitingTime;
		UTRSManager.Instance.leftTimeText.text = UTRSManager.Instance.leftTime.ToString("N0");
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (UTRSManager.Instance.leftTime > 0 && stateInfo.normalizedTime >= 1f)
		{
			UTRSManager.Instance.leftTime -= Time.deltaTime;
			UTRSManager.Instance.leftTimeText.text = UTRSManager.Instance.leftTime.ToString("N0");
		}

		if (UTRSManager.Instance.leftTime < 0)
		{ animator.SetBool("Generate", true); }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		temp = false;
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
