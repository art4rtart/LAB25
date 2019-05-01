using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
	Animator animator;

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
		}

		else return;
	}

	float openTime;
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
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
