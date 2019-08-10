using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myAgent : MonoBehaviour
{
	public LayerMask GameObject;
	public LabAgent labAgent;
	public float range = 15f;
	public float damping = 10f;
	public float sight = 100f;

	private int smallestIdx;
	private float distance;
	private float prevDistance;

	void Update()
	{
		RaycastHit hit;
		if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, sight))
		{

		}

		Vector3 forward = transform.TransformDirection(Vector3.forward) * sight;
		Debug.DrawRay(transform.position, forward, Color.green);

		Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, GameObject);
		prevDistance = 9999f;
	
		if (hitColliders.Length != 0)
		{
			for (int i = 0; i < hitColliders.Length; ++i)
			{
				distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);

				if (distance < prevDistance)
				{
					smallestIdx = i;
					prevDistance = distance;
				}
			}
			//Debug.Log(hitColliders[smallestIdx].gameObject);
			labAgent.target = hitColliders[smallestIdx].transform.gameObject;
			Vector3 lookPos = hitColliders[smallestIdx].transform.position - transform.position;
			lookPos.y = 0;
			Quaternion rotation = Quaternion.LookRotation(lookPos);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
