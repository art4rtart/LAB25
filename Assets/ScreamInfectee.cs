using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScreamInfectee : MonoBehaviour
{
	AudioSource audioSource;
	Animator anim;
	NavMeshAgent navmesh;

	public float findRadius;
	public LayerMask playerMask;

	public float getZombieRadius;
	public LayerMask enemyMask;
	public Renderer rend;

	public Transform destination;

	Transform player;

	void Awake()
	{
		player = FindObjectOfType<PlayerCtrl>().transform;
		audioSource = GetComponent<AudioSource>();
		navmesh = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
		StartCoroutine(Scream());
	}

	IEnumerator Scream()
	{
		while(true)
		{
			Collider[] playerInRadius = Physics.OverlapSphere(transform.position, findRadius, playerMask);

			if (playerInRadius.Length != 0) break;
			yield return null;
		}

		// Go
		anim.SetTrigger("Move");

		navmesh.SetDestination(destination.position);
		yield return new WaitForSeconds(.25f);

		//arrive
		while (navmesh.remainingDistance != 0) { this.transform.LookAt(player);  yield return null; }
		anim.SetTrigger("Stop");
		yield return new WaitForSeconds(.5f);
		anim.SetTrigger("Scream");
		audioSource.Play();

		Collider[] enemyInRadius = Physics.OverlapSphere(transform.position, getZombieRadius, enemyMask);

		for (int i = 0; i < enemyInRadius.Length; i++)
		{
			StartCoroutine(enemyInRadius[i].GetComponent<Her0inEnemy>().Follow());
			enemyInRadius[i].GetComponent<Her0inEnemy>().followTarget = true;
		}
		yield return null;

		// Die Dissolve
		yield return new WaitForSeconds(1.75f);
		StartCoroutine(Dissolve());
	}

	IEnumerator Dissolve()
	{
		float value = 0;

		while (value <= 1)
		{
			value += Time.deltaTime * 1.5f;
			rend.material.SetFloat(Shader.PropertyToID("_Dissolved"), value);
			yield return null;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, findRadius);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, getZombieRadius);
	}
}
