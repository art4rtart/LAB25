using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Limpid : MonoBehaviour
{
	public Material mat1;
	public Material mat2;

	Animator animator;
	NavMeshAgent navmesh;
	public SpawnEffect spawnEffect;

	public float viewRadius;
	public LayerMask targetMask;
	public Transform target;
	Transform jumpTarget;

	bool setTarget;
	bool jumped;
	bool jumpTrigger;

	public float flyingSpeed = 5f;
	bool firstLoop = false;

	// 모델이 바뀌면 수정해야될 것
	// remaining distance -> 현재 9f, 애니메이션 시간 -> guswo .43f

	// Start is called before the first frame update
	void Start()
    {
		navmesh = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
	}

	public Renderer limpidRenderer;

	public void ChangeToNormalMaterial()
	{
		limpidRenderer.material = mat2;
	}

	public void ChangeToVisionMaterial()
	{
		limpidRenderer.material = mat1;
	}

    // Update is called once per frame
    void Update()
    {
		Collider[] humanInRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		if (humanInRadius.Length > 0)
		{
			if(!jumped)
				target = humanInRadius[0].transform;

			if (!setTarget)
			{
				animator.SetTrigger("Run");
				spawnEffect.enabled = true;
				navmesh.enabled = true;
				setTarget = true;
			}
		}

		else
			target = null;

		if (target != null)
		{
			if (!jumped)
			{
				navmesh.SetDestination(target.position);
			}

			else
			{
				navmesh.enabled = false;
			}

			if (navmesh.enabled)
			{
				float distance = navmesh.remainingDistance;
				
				if (distance < 9f && firstLoop && !jumped)
				{
					animator.SetTrigger("Jump");
					jumped = true;
				}
			}

			else
			{
				if (!jumpTrigger)
				{
					jumpTarget = target;
					StartCoroutine(JumpToHuman(jumpTarget));
					jumpTrigger = true;
				}
			}

			firstLoop = true;
		}
	}


	IEnumerator JumpToHuman(Transform jumpTarget)
	{
		Vector3 landingPosition = jumpTarget.position;

		yield return new WaitForSeconds(0.5f);

		while (animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.43f)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, landingPosition, flyingSpeed * Time.deltaTime);

			yield return null;
		}

		animator.SetTrigger("Idle");
		yield return new WaitForSeconds(2.0f);
		Debug.Log("OK");
		jumpTrigger = false;
		jumped = false;
		firstLoop = false;
		setTarget = false;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, viewRadius);
	}
}
