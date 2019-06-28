using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAgent : MonoBehaviour
{
	Rigidbody rgbd;
	FindBlockAgent findBlockAgent;

	bool check;

	void Awake()
	{
		rgbd = GetComponent<Rigidbody>();
		findBlockAgent = GetComponent<FindBlockAgent>();
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag("ground"))
		{
			if (!check)
			{
				transform.position = new Vector3(this.transform.position.x, 3.5f, this.transform.position.z);
				rgbd.constraints = RigidbodyConstraints.FreezeRotation;
				findBlockAgent.enabled = true;
				check = true;
			}
			this.enabled = false;
		}
	}
}
