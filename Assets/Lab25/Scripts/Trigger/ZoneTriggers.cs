using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTriggers : MonoBehaviour
{
	public bool isColliding = false;

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			BombGage.Instance.canInstall = true;
			if(!BombInstallInstructor.Instance.enabled) BombInstallInstructor.Instance.enabled = true;
			BombInstallInstructor.Instance.isOutOfBound = false;
			isColliding = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			BombGage.Instance.canInstall = false;
			BombInstallInstructor.Instance.isOutOfBound = true;
			isColliding = false;
		}
	}

	public void changeZoneCircleRange()
	{
		StartCoroutine(this.gameObject.transform.parent.GetComponent<AttackRange>().DecreaseCircle());
	}
}
