using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
	Animator anim;
	Rigidbody rgbd;

	public Transform target;
	public float runSpeed;
	public float turnSpeed;
	public float lookTurnSpeed = 40f;
	float runLerpSpeed;

	void Awake()
	{
		anim = GetComponent<Animator>();
		rgbd = GetComponent<Rigidbody>();
		csCollider = GetComponent<CapsuleCollider>();
		StartCoroutine(FindAndScream(target.transform.position));
	}

	bool islockTarget = true;
	void Update()
	{
		AnimatorStateInfo animStateInfo = anim.GetCurrentAnimatorStateInfo(0);

		if (animStateInfo.IsName("Run"))
		{
			float distance = Vector3.Distance(this.transform.position, target.transform.position);

			if (distance < 8f)
			{
				islockTarget = false;
			}

			else
			{
				if (islockTarget)
				{
					Vector3 dirToLookTarget = (target.transform.position - this.transform.position).normalized;
					float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

					if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
					{
						float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, lookTurnSpeed * Time.deltaTime);
						transform.eulerAngles = Vector3.up * angle;
					}
				}
			}

			runSpeed = Mathf.Lerp(0, 15f, runLerpSpeed);
			runLerpSpeed += Time.deltaTime;
			transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
		}
	}

	bool fallbackTrigger;

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("wall"))
		{
			if (fallbackTrigger == false) {
				rgbd.mass = 100f;
				rgbd.drag = 50f;
				StartCoroutine(Fallback());
				fallbackTrigger = true;
			}
		}
	}

	CapsuleCollider csCollider;
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("wall"))
		{
			if (fallbackTrigger == false)
			{
				csCollider.isTrigger = false;
			}
		}
	}

	IEnumerator Fallback()
	{
		rgbd.isKinematic = true;
		anim.SetTrigger("HitWall");
		anim.SetBool("Run", false);
		runSpeed = 0f;
		runLerpSpeed = 0;

		float tempSpeed = 4f;
		float lerpSpeed = 1.25f;
		float lerpTime = 0;

		while (tempSpeed > 0f)
		{
			transform.Translate(Vector3.back * tempSpeed * Time.deltaTime);
			tempSpeed = Mathf.Lerp(8f, 0, lerpTime);
			lerpTime += lerpSpeed * Time.deltaTime;
			yield return null;
		}

		rgbd.constraints = RigidbodyConstraints.FreezePosition;

		yield return new WaitForSeconds(1f);

		rgbd.constraints = RigidbodyConstraints.None;
		rgbd.constraints = RigidbodyConstraints.FreezeAll;

		anim.SetTrigger("Turn");
		StopAllCoroutines();
		StartCoroutine(TurnToFace(target.position));
	}

	public IEnumerator FindAndScream(Vector3 lookTarget)
	{
		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
		{
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}

		anim.SetTrigger("Scream");
		yield return null;
	}

	public IEnumerator TurnToFace(Vector3 lookTarget)
	{
		fallbackTrigger = false;
		islockTarget = true;
		csCollider.isTrigger = true;

		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
		{
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}

		anim.SetBool("Run", true);
		rgbd.isKinematic = false;
		yield return null;
	}

	public void Die()
	{
		anim.SetTrigger("Die");
		Invoke("Destroy", 2f);
	}

	void Destroy()
	{
		this.gameObject.SetActive(false);
	}
}
