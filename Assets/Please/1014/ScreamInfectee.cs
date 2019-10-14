using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamInfectee : MonoBehaviour
{
	AudioSource audioSource;
	Animator anim;

	public float findRadius;
	public LayerMask playerMask;

	public float getZombieRadius;
	public LayerMask enemyMask;
	public Renderer rend;

	Transform player;

	void Awake()
	{
		player = FindObjectOfType<PlayerCtrl>().transform;
		audioSource = GetComponent<AudioSource>();
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

		anim.SetTrigger("Scream");
		
		Collider[] enemyInRadius = Physics.OverlapSphere(transform.position, getZombieRadius, enemyMask);

		for (int i = 0; i < enemyInRadius.Length; i++)
		{
			StartCoroutine(enemyInRadius[i].GetComponent<Her0inEnemy>().Follow());
			enemyInRadius[i].GetComponent<Her0inEnemy>().followTarget = true;
		}
		yield return null;

		yield return new WaitForSeconds(.5f);
		audioSource.Play();

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
