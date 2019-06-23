using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWall : MonoBehaviour
{
	BoxCollider bxCollider;
	public bool twice;
	public int count = 0;

	void Awake()
	{
		bxCollider = GetComponent<BoxCollider>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("agent"))
		{
			if(!twice) this.gameObject.tag = "wall";
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("agent"))
		{
			if(twice)
			{
				count++;
				if (count >= 2)
				{
					this.gameObject.tag = "wall";
					bxCollider.isTrigger = false;
				}
			}

			else
				bxCollider.isTrigger = false;
		}
	}
}