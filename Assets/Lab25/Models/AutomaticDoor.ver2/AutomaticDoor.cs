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

	[Header("NoOpen Gate Only")]
	public bool isTriggerGate;
	public Sensor sensor;
	public Renderer[] rend;
	public Color currentColor;
	public Color targetColor;
	float duration = 2; // This will be your time in seconds.
	float smoothness = 0.05f;

	IEnumerator colorLerpBlue;
	IEnumerator colorLerpRed;

	void Start()
	{
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		colorLerpBlue = LerpColor(currentColor, targetColor);
		colorLerpRed = LerpColor(targetColor, currentColor);
	}

	void OnTriggerEnter(Collider other)
	{
		if(!isOpened && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Infectee")))
		{
			if(!isTriggerGate) OpenDoor();
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

	public void OpenDoor()
	{
		audioSource.Stop();
		animator.SetBool("DoorOpen", true);
		audioSource.clip = openSound;
		audioSource.Play();
		StartCoroutine(OpenTime());
		isOpened = true;

		if (!isTriggerGate) return;
		StopCoroutine(colorLerpRed);
		StartCoroutine(colorLerpBlue);
	}

	IEnumerator OpenTime()
	{
		yield return new WaitForSeconds(1.0f);

		while (isColliding && !isTriggerGate)
		{
			yield return null;
		}

		if (sensor != null) { while (sensor.isCollidingWithMoveableObject) { yield return null; } }

		yield return new WaitForSeconds(2.0f);
		isOpened = false;
		audioSource.Stop();
		animator.SetBool("DoorOpen", false);
		audioSource.clip = closeSound;
		audioSource.Play();
		if (isTriggerGate) { StopCoroutine(colorLerpBlue); StartCoroutine(colorLerpRed); }
		if (sensor != null) sensor.isOpend = false;
	}

	IEnumerator LerpColor(Color current, Color target)
	{
		MaterialPropertyBlock mpb = new MaterialPropertyBlock();

		float progress = 0;
		float increment = smoothness / duration;

		while (progress < 1)
		{
			currentColor = Color.Lerp(current, target, progress);
			mpb.SetColor(Shader.PropertyToID("_Color"), currentColor);
			for (int i = 0; i < rend.Length; i++) rend[i].SetPropertyBlock(mpb);
			progress += increment;
			yield return new WaitForSeconds(smoothness);
		}

		yield return null;
	}
}
