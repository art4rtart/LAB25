using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Her0inEnemy : MonoBehaviour
{
	Animator anim;
	Rigidbody rgbd;

	public LayerMask humanMask;
	public float findRadius;
	public float attackRadius;

	NavMeshAgent navMesh;
	Transform target;
	bool followTarget;
	public bool isGenerated = false;

	void OnEnable()
	{
		if (isGenerated)
		{
			target = GameObject.FindWithTag("Player").transform;
		}
	}

	void Awake()
    {
		anim = GetComponent<Animator>();
		rgbd = GetComponent<Rigidbody>();
		navMesh = GetComponent<NavMeshAgent>();
		myChange = GetComponentInParent<ChangeRagDoll>();
		csCollider = GetComponent<CapsuleCollider>();
	}

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
		anim.SetTrigger("Attack");
		yield return new WaitForSeconds(1f);

		navMesh.SetDestination(target.transform.position);

		if (navMesh.remainingDistance <= navMesh.stoppingDistance)
			StartCoroutine(Attack());

		else
			yield return null;

		yield return null;
	}

	//private void OnDrawGizmosSelected()
	//{
	//	Gizmos.color = Color.red;
	//	Gizmos.DrawWireSphere(transform.position, findRadius);
	//}

	//infectee attributes
	public int hp;
	public int maxHp;
	private ChangeRagDoll myChange;
	public RagDollDIeCtrl myRagDollCtrl;
	public Vector3 hitPos;

	public void ApplyDamage(int damage)
	{
		hp -= damage;

		if (hp <= 0)
		{
			myRagDollCtrl.speed = navMesh.velocity.magnitude;
			myRagDollCtrl.AttackedPos = hitPos;
			Die();
		}
	}

	private void Die()
	{
		myChange.StartCoroutine(myChange.ChangeRagdoll());
	}

	CapsuleCollider csCollider;
	bool settingTrigger;

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			if (!settingTrigger)
			{
				rgbd.constraints = RigidbodyConstraints.FreezePositionY;
				csCollider.isTrigger = true;
				navMesh.enabled = true;

				if (isGenerated)
				{
					anim.SetBool("Run", true);
					followTarget = true;
				}

				settingTrigger = true;
			}

			else return;
		}
	}
}
