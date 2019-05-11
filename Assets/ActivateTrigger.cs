using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
	public GameObject activateObject;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if(!activateObject.activeSelf) activateObject.SetActive(true);
		}
	}
}
