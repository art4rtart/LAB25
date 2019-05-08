using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
	Animator animator;
	bool doorOpen;

	void Awake()
	{
		animator = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Infectee"))
		{
			StopAllCoroutines();
			animator.SetBool("Open", true);
			doorOpen = true;
		}

		else return;
	}

	//void OnTriggerStay(Collider other)
	//{
	//	if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Infectee"))
	//	{
	//		doorOpen = true;
	//	}

	//	else doorOpen = false;
	//}

	float openTime;
	void OnTriggerExit(Collider other)
	{
		doorOpen = false;
		if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Infectee")) && !doorOpen)
		{
			StartCoroutine(CloseDoor());
		}

		else return;
	}

	IEnumerator CloseDoor()
	{
		while (openTime < 2)
		{
			openTime += Time.deltaTime;

			if (openTime >= 2f)
			{
				animator.SetBool("Open", false);
				openTime = 0;
			}

			yield return null;
		}
	}
}
