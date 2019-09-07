using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LimpidMovement : MonoBehaviour
{
	Animator anim;
	NavMeshAgent navMesh;

	Transform target;
	public Transform[] waypoints;

	void Awake()
	{
		anim = GetComponent<Animator>();
	}

	void Start()
	{
		navMesh = GetComponent<NavMeshAgent>();
		anim.SetBool("Walk", true);
		target = waypoints[0];
		navMesh.SetDestination(target.position);
		navMesh.enabled = true;
	}

	float timeCount;

	void Update()
	{
		if (Vector3.Distance(this.transform.position, target.position) <= 0.1f)
		{
			anim.SetBool("Walk", false);
			navMesh.SetDestination(this.transform.position);
			timeCount += Time.deltaTime;
		}

		if(timeCount > 15)
		{
			target = waypoints[1];
			anim.SetBool("Walk", true);
			navMesh.SetDestination(target.position);
			timeCount = 0;
		}
	}
}
