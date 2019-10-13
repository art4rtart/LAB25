using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Beaker : MonoBehaviour
{
	[Header("BeakerSettings")]
	public GameObject destroyedBeaker;

	[Header("BrokenBeakerSettings")]
	Rigidbody rgbd;
	public Her0inEnemy[] infectees;
	public bool isBrokenBeaker;

	void Awake()
	{
		if (!isBrokenBeaker) return;
		for (int i = 0; i < infectees.Length; i++)
		{
			infectees[i].target = this.gameObject.transform;
			infectees[i].player = this.gameObject;
			if (!infectees[i].GetComponent<NavMeshAgent>().enabled) infectees[i].GetComponent<NavMeshAgent>().enabled = true;
			infectees[i].GetComponent<NavMeshAgent>().SetDestination(infectees[i].target.position);
			StartCoroutine(infectees[i].Follow());
			infectees[i].followTarget = true;
		}

		for (int i = 0; i < this.gameObject.transform.childCount; i++)
		{
			this.gameObject.transform.GetChild(i).GetComponent<Rigidbody>().AddForce(Vector3.up * -10000f);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (isBrokenBeaker) return;
		destroyedBeaker.SetActive(true);
		destroyedBeaker.transform.SetParent(null);
		this.gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Infectee") && isBrokenBeaker)
		{
			other.gameObject.GetComponent<Animator>().SetBool("Run", false);
			other.gameObject.GetComponent<NavMeshAgent>().speed = 0;
		}
	}
}
