using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Her0inEnemy : MonoBehaviour
{
	Animator anim;

	public LayerMask humanMask;
	public float findRadius;


	public float attackRadius;

	NavMeshAgent navMesh;

	void Awake()
    {
		anim = GetComponent<Animator>();
		navMesh = GetComponent<NavMeshAgent>();
	}

	Transform target;
	bool followTarget;

	void Update()
    {
		AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

		if (!followTarget)
		{
			Collider[] humanInRadius = Physics.OverlapSphere(transform.position, findRadius, humanMask);

			for (int i = 0; i < humanInRadius.Length; i++)
			{
				if (humanInRadius[i].transform.CompareTag("Player"))
				{
					target = humanInRadius[i].transform;

					anim.SetBool("Run", true);
					followTarget = true;
				}
			}
		}

		if(followTarget)
		{
			if (navMesh.remainingDistance != 0 && navMesh.remainingDistance <= navMesh.stoppingDistance)
			{
				if (!corTrigger)
				{
					anim.SetBool("Run", false);
					navMesh.SetDestination(this.transform.position);
					StartCoroutine(Attack());
					corTrigger = true;
				}

				Vector3 calcuatledtarget = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
				transform.LookAt(calcuatledtarget);
			}

			else
			{
				corTrigger = false;
				anim.SetBool("Run", true);
				navMesh.SetDestination(target.transform.position);
			}
		}

		if(Input.GetKeyDown(KeyCode.V))
		{
			m = true;
		}
	}

	bool m = false;

	bool corTrigger;

	IEnumerator Attack()
	{
		Debug.Log("HI");
		anim.SetTrigger("Attack");
		yield return new WaitForSeconds(1f);

		navMesh.SetDestination(target.transform.position);
		Debug.Log(navMesh.remainingDistance);

		if (navMesh.remainingDistance <= navMesh.stoppingDistance)
			StartCoroutine(Attack());
		else
			StopAllCoroutines();
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, findRadius);
	}
}
