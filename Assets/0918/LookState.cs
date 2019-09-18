using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookState : StateMachineBehaviour
{
	PlayerCtrl playerController;
	GameObject crossHair;
	GameObject HUD;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		crossHair = FindObjectOfType<Crosshair>().gameObject;
		HUD = FindObjectOfType<UIManager>().transform.GetChild(3).gameObject;
		playerController = FindObjectOfType<PlayerCtrl>();
		
		playerController.enabled = false;
		crossHair.SetActive(false);
		HUD.SetActive(false);
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//    
	//}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		playerController.enabled = true;
		crossHair.SetActive(true);
		HUD.SetActive(true);
		animator.enabled = false;
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
