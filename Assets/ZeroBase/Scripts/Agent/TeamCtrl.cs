using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeamCtrl : MonoBehaviour
{
    NavMeshAgent navmesh;
    GameObject Player;
    public bool isMyTeam;
    public bool added;
    public bool targetToInfectee = false;
    public float teamTimeCount = 10f;

    public float enemyFindRadius;
    public LayerMask playerMask;
    public LayerMask enemyMask;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        navmesh = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        //StartCoroutine(FollowPlayer());
    }

    void Update()
    {
        Collider[] enemyInRadius = Physics.OverlapSphere(transform.position, enemyFindRadius, enemyMask);
        targetToInfectee = false;
        for (int i = 0; i < enemyInRadius.Length; i++)
        {
            if (enemyInRadius[i].transform.CompareTag("Infectee"))
            {
                targetToInfectee = true;
                Vector3 enemyPos = new Vector3(enemyInRadius[i].transform.position.x, this.transform.position.y, enemyInRadius[i].transform.position.z);

                Vector3 dirToTarget = enemyPos - transform.position;
                Vector3 look = Vector3.Slerp(transform.forward, dirToTarget.normalized, Time.deltaTime * 10);
                //Debug.Log("LootInfectee");
                transform.rotation = Quaternion.LookRotation(look, Vector3.up);
            }
        }

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

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(this.transform.position, enemyFindRadius);
    //}
}
