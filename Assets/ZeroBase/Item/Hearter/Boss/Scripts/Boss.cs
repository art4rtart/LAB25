using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
	[Header("Default")]
	FieldOfView fov;
	NavMeshAgent navMesh;
	Rigidbody rgbd;

	[HideInInspector]
	public Vector3 direction;

	public float moveSpeed = 1f;
	public float turnSpeed = 180f;

	float lerpFactor;
	bool RageToTarget;
	public float lerpSpeed = 2f;

	Animator animator;

	void Start()
	{
		animator = GetComponent<Animator>();
		rgbd = GetComponent<Rigidbody>();
		navMesh = GetComponent<NavMeshAgent>();
		fov = GetComponent<FieldOfView>();
	}

	void Update()
	{
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("ZombieScream"))
		{
			Vector3 dirToLookTarget = (fov.target.transform.position - transform.position).normalized;
			float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

			if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
			{
				float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
				transform.eulerAngles = Vector3.up * angle;
			}

			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
			{
				animator.SetBool("Run", true);
				direction = (fov.target.transform.position - gameObject.transform.position).normalized * moveSpeed;

				if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
					RageToTarget = true;
			}
		}
	}

	void FixedUpdate()
	{
		if (!RageToTarget)
			return;

		else
		{
			rgbd.MovePosition(rgbd.position + direction * Time.fixedDeltaTime);
			//lerpFactor += lerpSpeed * Time.fixedDeltaTime;
		}
	}

	public float backForce;
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			if (RageToTarget)
			{
				animator.SetBool("Run", false);
				rgbd.AddForce(direction.normalized * -backForce, ForceMode.VelocityChange);

				if (fov.isTargetFound)
				{
					animator.SetTrigger("Scream");
					fov.isTargetLocked = false;
				}

				else
				{
					StartCoroutine(Growl());
				}

				lerpFactor = 0;
				RageToTarget = false;
			}
		}
	}

	public IEnumerator TurnToFace(Vector3 lookTarget)
	{
		Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
		{
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}

		animator.SetTrigger("Scream");
	}

	public IEnumerator UpdatePath(GameObject target)
	{
		float refreshRate = .25f;

		while (!fov.isTargetFound)
		{
			Vector3 targetPosition = new Vector3(target.transform.position.x, 0, target.transform.position.z);
			navMesh.SetDestination(targetPosition);
			yield return new WaitForSeconds(refreshRate);
		}

		if(fov.isTargetFound)
		{
			animator.SetBool("Find", false);
			navMesh.enabled = false;
			animator.SetBool("Run", true);

			while(animator.GetCurrentAnimatorStateInfo(0).IsName("ZombieScream") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.9f)
			{
				yield return null;
			}

			fov.isTargetLocked = false;
		}
	}

	public IEnumerator Growl()
	{
		yield return new WaitForSeconds(3f);
		navMesh.enabled = true;

		// Turn arround
		Vector3 dirToLookTarget = (fov.target.transform.position - transform.position).normalized;
		float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

		while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
		{
			float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
			transform.eulerAngles = Vector3.up * angle;
			yield return null;
		}

		// Growl effect
		if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) < 0.05f)
		{
			animator.SetBool("Find", true);
			StartCoroutine(UpdatePath(fov.target));
		}
	}
}
