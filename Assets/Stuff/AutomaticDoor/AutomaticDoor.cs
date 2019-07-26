using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
	private AudioSource audioSource;
	private Animator animator;

	public AudioClip openSound;
	public AudioClip closeSound;

	bool isOpened;
	public bool isColliding;

	void Start()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(!isOpened && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Infectee")))
		{
			audioSource.Stop();
			animator.SetBool("DoorOpen", true);
			audioSource.clip = openSound;
			audioSource.Play();
			StartCoroutine(OpenTime());
			isOpened = true;
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Infectee"))
		{
			isColliding = true;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Infectee"))
		{
			isColliding = false;
		}
	}

	IEnumerator OpenTime()
	{
		yield return new WaitForSeconds(1.0f);

		while (isColliding)
		{
			yield return null;
		}

		yield return new WaitForSeconds(2.0f);
		isOpened = false;
		audioSource.Stop();
		animator.SetBool("DoorOpen", false);
		audioSource.clip = closeSound;
		audioSource.Play();
	}
}
