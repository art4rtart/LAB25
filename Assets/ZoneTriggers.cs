using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTriggers : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			BombGage.canInstall = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			BombGage.canInstall = false;
		}
	}

}
