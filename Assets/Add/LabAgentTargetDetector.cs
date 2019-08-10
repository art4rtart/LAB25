using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabAgentTargetDetector : MonoBehaviour
{
	public LayerMask ObjectMask;
	public LabAgent labAgent;
	public float range = 15f;
	public float damping = 10f;
	public float sight = 100f;

	private int smallestIdx;
	private float distance;
	private float prevDistance;

	void Update()
	{
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, ObjectMask);
		//prevDistance = 9999f;

		if (hitColliders.Length == 0)
		{
			labAgent.enabled = false;
		}

		else
		{
			for (int i = 0; i < hitColliders.Length; ++i)
			{
				transform.LookAt(hitColliders[i].transform.position);
				labAgent.target = hitColliders[i].transform.gameObject;
				labAgent.feature = hitColliders[i].GetComponent<Feature>();
				if (labAgent.target != null && labAgent.feature != null) labAgent.enabled = true;
				else labAgent.enabled = false;
			}
		}
	}

	void OnDrawGizmosSelected()
	{
		Vector3 forward = transform.TransformDirection(Vector3.forward) * sight;
		Debug.DrawRay(transform.position, forward, Color.green);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
