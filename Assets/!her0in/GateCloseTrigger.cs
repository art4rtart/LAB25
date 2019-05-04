using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCloseTrigger : MonoBehaviour
{
	public Animator gateAnimator;
	bool closed = false;
	public GameObject sector;
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !closed)
		{
			gateAnimator.SetBool("GateOpen", false);
			sector.SetActive(false);
			closed = true;
		}
	}
}
