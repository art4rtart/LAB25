using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTriggers : MonoBehaviour
{
	public bool isColliding = false;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			BombGage.canInstall = true;
			isColliding = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			BombGage.canInstall = false;
			isColliding = false;
		}
	}

	public void changeZoneCircleRange()
	{
		StartCoroutine(this.gameObject.transform.parent.GetComponent<AttackRange>().DecreaseCircle());
	}
}
