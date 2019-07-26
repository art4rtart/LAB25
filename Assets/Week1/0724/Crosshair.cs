using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
	Animator animator;
	public RectTransform crosshair;

	public bool locked;
	public float rotateSpeed = 20f;

	IEnumerator targetLock;
	IEnumerator targetUnlock;

	void Awake()
	{
		animator = GetComponent<Animator>();
		targetLock = TargetLocked();
		targetUnlock = TargetUnLocked();

		StartCoroutine(targetUnlock);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.L))
		{
			locked = !locked;
		}
	}

	public void LockCrosshair()
	{
		locked = !locked;
	}

	IEnumerator TargetLocked()
	{
		animator.SetBool("Lock", locked);
		yield return new WaitForSeconds(.075f);
		crosshair.rotation = Quaternion.identity;

		while (locked)
		{
			yield return null;
		}

		StartCoroutine(TargetUnLocked());
	}

	IEnumerator TargetUnLocked()
	{
		animator.SetBool("Lock", locked);

		while (!locked)
		{
			crosshair.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
			yield return null;
		}

		StartCoroutine(TargetLocked());
	}
}
