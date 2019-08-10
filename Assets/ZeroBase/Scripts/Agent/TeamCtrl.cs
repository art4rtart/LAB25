using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeamCtrl : MonoBehaviour
{
    NavMeshAgent navmesh;
    public GameObject Player;
	Rigidbody rgbd;

    public bool isMyTeam;
    public bool added;
    public bool targetToInfectee = false;
    public float teamTimeCount = 10f;

    public float enemyFindRadius;
    public LayerMask playerMask;
    public LayerMask enemyMask;
    Animator animator;
	AgentShoot agentShoot;

	public LabAgent labAgent;
    void Start()
    {
        animator = GetComponent<Animator>();
        navmesh = GetComponent<NavMeshAgent>();
        //Player = GameObject.FindGameObjectWithTag("Player");
		rgbd = GetComponent<Rigidbody>();
		agentShoot = GetComponent<AgentShoot>();
	}

    void Update()
    {
        Collider[] enemyInRadius = Physics.OverlapSphere(transform.position, enemyFindRadius, enemyMask);
        targetToInfectee = false;

		if (enemyInRadius.Length == 0)
		{
			labAgent.enabled = false;
		}

		else
		{
			for (int i = 0; i < enemyInRadius.Length; ++i)
			{
				transform.LookAt(enemyInRadius[i].transform.position);
				labAgent.target = enemyInRadius[i].transform.gameObject;
				labAgent.feature = enemyInRadius[i].GetComponent<Feature>();
				if (labAgent.target != null && labAgent.feature != null) labAgent.enabled = true;
				else labAgent.enabled = false;
			}
		}

		//if (isMyTeam) agentShoot.enabled = true;
		//Collider[] enemyInRadius = Physics.OverlapSphere(transform.position, enemyFindRadius, enemyMask);

			//for (int i = 0; i < enemyInRadius.Length; i++)
			//{
			//    if (enemyInRadius[i].transform.CompareTag("Infectee"))
			//    {
			//        Vector3 enemyPos = new Vector3(enemyInRadius[i].transform.position.x, this.transform.position.y, enemyInRadius[i].transform.position.z);
			//        this.gameObject.transform.LookAt(enemyPos);
			//    }
			//}
	}

    public IEnumerator FollowPlayer()
    {
        float refreshRate = .25f;
        animator.SetBool("isRun", true);

        while (Player != null)
        {

            navmesh.SetDestination(Player.transform.position);

            if (navmesh.remainingDistance < navmesh.stoppingDistance)
                animator.SetBool("isRun", false);

            else
                animator.SetBool("isRun", true);

            float distance = Vector3.Distance(this.gameObject.transform.position, Player.transform.position);

            yield return new WaitForSeconds(refreshRate);
        }
    }

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, enemyFindRadius);
	}

	bool enableTrigger;
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag("Floor"))
		{
			if(!enableTrigger)
			{
				rgbd.mass = 100f;
				rgbd.drag = 50f;
				rgbd.constraints = RigidbodyConstraints.FreezeAll;
				navmesh.enabled = true;
				enableTrigger = true;
			}
		}
	}
}
