﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Her0inEnemy : MonoBehaviour
{
	Animator anim;
	Rigidbody rgbd;
	NavMeshAgent navMesh;
	Transform target;
	CapsuleCollider csCollider;

	[Header("Infectee")]
	public LayerMask humanMask;
	public float findRadius;
	bool followTarget;
	bool settingTrigger;
	bool corTrigger;
	public bool isAttack;
	public float attackRange;
	bool foundTarget;

	[Header("if generated by generator check")]
	public bool isGenerated = false;

	[Header("Limpid")]
	public bool isLimpid;
	public SpawnEffect spawnEffect;
	public DamagedEffect damagedEffect;

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

		if (!followTarget && !isGenerated)
		{
			Collider[] humanInRadius = Physics.OverlapSphere(transform.position, findRadius, humanMask);

			for (int i = 0; i < humanInRadius.Length; i++)
			{
				if (humanInRadius[i].transform.CompareTag("Player"))
				{
					target = humanInRadius[i].transform;
					if(navMesh.isOnNavMesh)navMesh.SetDestination(target.position);
					StartCoroutine(Follow());
					followTarget = true;
				}
			}
		}

		if (navMesh.enabled && target != null) navMesh.SetDestination(target.transform.position);

		if (info.IsName("Attack") || info.IsName("Run"))
		{
			Vector3 calcuatledtarget = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
			transform.LookAt(calcuatledtarget);
		}

		if (isLimpid && navMesh.enabled && navMesh.remainingDistance != 0&& navMesh.remainingDistance < navMesh.stoppingDistance + 10f && !isDissolved)
		{
			spawnEffect.enabled = true;
			isDissolved = true;
		}
	}

	public float moveSpeed;
	public bool isDissolved;
	IEnumerator Follow()
	{
		navMesh.speed = moveSpeed;
		anim.SetBool("Run", true);
		navMesh.enabled = true;

		while(navMesh.remainingDistance >= navMesh.stoppingDistance)
		{
			yield return null;
		}

		navMesh.enabled = false;
		anim.SetBool("Run", false);

		StartCoroutine(Attack());
	}

	public float attackSpeed;

	IEnumerator Attack()
	{
		if (!foundTarget)
		{
			foundTarget = true;
		}

		else
		{
			anim.SetTrigger("Attack");
		}

		yield return new WaitForSeconds(attackSpeed);

		navMesh.speed = 0f;
		navMesh.enabled = true;

		while (navMesh.remainingDistance == 0)
			yield return null;

		if (navMesh.enabled && navMesh.remainingDistance >= navMesh.stoppingDistance)
		{
			StartCoroutine(Follow());
		}

		if (navMesh.enabled && navMesh.remainingDistance <= navMesh.stoppingDistance)
		{
			StartCoroutine(Attack());
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (!isGenerated)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, findRadius);
		}

		else return;
	}

	//infectee attributes
	public int hp;
	public int maxHp;
	private ChangeRagDoll myChange;
	public RagDollDIeCtrl myRagDollCtrl;
	[HideInInspector] public Vector3 hitPos;

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

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Wall"))
		{
			if (!settingTrigger)
			{
				rgbd.constraints = RigidbodyConstraints.FreezePositionY;
				rgbd.mass = 100;
				rgbd.drag = 50;

				if (isGenerated) StartCoroutine(Follow());

				settingTrigger = true;
			}

			else return;
		}
	}
}