using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cinematic : MonoBehaviour
{
	[Header("Limpid")]
	public SpawnEffect spawnEffect;
	public Animator limpidCinematicAnimator;
	public NavMeshAgent navmesh;
	public Transform destination;
	public Transform limpid;
	Camera cam;

	void Awake()
	{
		cam = Camera.main;

		navmesh.SetDestination(destination.position);
		limpidCinematicAnimator.SetBool("Walk", true);
	}

	bool stopTrigger = false;

	void Update()
	{
		if (navmesh.enabled)
		{
			if (navmesh.remainingDistance <= 2f)
			{
				if (!stopTrigger)
				{
					Debug.Log("OK");
					limpidCinematicAnimator.SetBool("Walk", false);
					//navmesh.enabled = false;
					stopTrigger = true;
				}
			}
		}

		if(Input.GetKeyDown(KeyCode.Space))
		{
			BGM.SetActive(true);
		}
	}

	public GameObject BGM;

	public void Dissolve()
	{
		spawnEffect.enabled = true;
	}

	public Camera hackCamrea;
	public void DetachCamera()
	{
		hackCamrea.transform.position = this.transform.position;
		hackCamrea.transform.rotation = this.transform.rotation;
		cam.enabled = false;
		hackCamrea.enabled = true;
	}
}
