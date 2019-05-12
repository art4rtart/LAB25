using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
	public GameObject door;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			door.GetComponent<Animator>().enabled = false;
			door.isStatic = true;
		}
	}
}
