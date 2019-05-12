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
		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Infectee") || other.gameObject.CompareTag("PlayerAgent"))
		{
			StopAllCoroutines();
			animator.SetBool("Open", true);
			doorOpen = true;
		}

		else return;
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Infectee") || other.gameObject.CompareTag("PlayerAgent"))
		{
			doorOpen = true;
		}

		else doorOpen = false;
	}

	float openTime;
	void OnTriggerExit(Collider other)
	{
		if ((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Infectee") || other.gameObject.CompareTag("PlayerAgent")) && !doorOpen)
		{
			StartCoroutine(CloseDoor());
		}

		else return;
	}

	IEnumerator CloseDoor()
	{
		while (openTime < 1f)
		{
			openTime += Time.deltaTime;

			if (openTime >= 1f)
			{
				animator.SetBool("Open", false);
				openTime = 0;
			}

			yield return null;
		}
	}
}
