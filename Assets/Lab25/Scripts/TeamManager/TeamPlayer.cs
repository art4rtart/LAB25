using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeamPlayer : MonoBehaviour
{
	[Header("Material Settings")]
	public Material defaultMaterial;
	public Material xrayMaterial;
	public Renderer teamRenderer;

	[Header("LayerMask Settings")]
	public LayerMask playerMask;
	public LayerMask enemyMask;
	public float findRadius;

	NavMeshAgent navmesh;
	Animator animator;
	GameObject Player;
	[HideInInspector] public bool isMyTeam;
	[HideInInspector] public bool recruited;

	void Awake()
    {
		animator = GetComponent<Animator>();
		navmesh = GetComponent<NavMeshAgent>();
		Player = GameObject.FindGameObjectWithTag("Player");
        // StartCoroutine(FollowPlayer());
    }

    void Update()
    {
		Collider[] enemyInRadius = Physics.OverlapSphere(transform.position, findRadius, enemyMask);

		for(int i =0; i < enemyInRadius.Length; i++)
		{
			if(enemyInRadius[i].transform.CompareTag("Enemy"))
			{
				Vector3 enemyPos = new Vector3(enemyInRadius[i].transform.position.x, this.transform.position.y, enemyInRadius[i].transform.position.z);
				this.gameObject.transform.LookAt(enemyPos);
			}
		}
	}

	public IEnumerator FollowPlayer()
	{
        Debug.Log("asd");
		float refreshRate = .25f;
		animator.SetBool("Walking", true);

		while (Player != null)
		{
			navmesh.SetDestination(Player.transform.position);

			if(navmesh.remainingDistance < navmesh.stoppingDistance)
				animator.SetBool("Walking", false);

			else
				animator.SetBool("Walking", true);

			float distance = Vector3.Distance(this.gameObject.transform.position, Player.transform.position);

			yield return new WaitForSeconds(refreshRate);
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 3);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, findRadius);
	}
}
