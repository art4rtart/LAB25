using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGoalDetect : MonoBehaviour
{
	[HideInInspector]
	public FindBlockAgent agent;

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("agent"))
		{
			agent.IScoredAGoal();
			this.gameObject.SetActive(false);
		}
	}
}
