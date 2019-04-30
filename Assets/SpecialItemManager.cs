using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialItemManager : MonoBehaviour
{
	public Animator playerAnim;
	public Animator heraterAnim;

	bool heraterActivateTrigger;
	void Start()
    {

	}

	void Update()
	{
		AnimatorStateInfo animStateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);

		if (animStateInfo.IsName("useHarter") && animStateInfo.normalizedTime > 0.2f && !heraterActivateTrigger)
		{
			heraterAnim.SetBool("activate", true);
			Debug.Log("Hearter activated");
			heraterActivateTrigger = true;
		}

		else if (animStateInfo.IsName("New State") && animStateInfo.normalizedTime > 0.5f && heraterActivateTrigger)
		{
			heraterAnim.SetBool("activate", false);
			Debug.Log("Hearter deactivated");
			heraterActivateTrigger = false;
		}
	}
}
