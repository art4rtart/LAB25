using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBarigateCollision : MonoBehaviour
{
	public bool isAddable = true;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Item"))
		{
			isAddable = false;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Item"))
		{
			isAddable = false;
		}
		else isAddable = true;
	}
}
