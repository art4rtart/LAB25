using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfecteeCtrl1 : MonoBehaviour
{
    //infectee attributes
    public int hp;
    public int maxHp;
    public int damage;
    //public float speed;
    //public int rotSpeed;
    public float attackRange;
    public float recognitionRange;


    //functional attributes
    private bool isAttack;
    private Transform target;

    //anim
    private Animator anim;
    private int hashFind = Animator.StringToHash("isFind");
    private int hashAttack1 = Animator.StringToHash("isAttack1");
    private int hashAttack2 = Animator.StringToHash("isAttack2");
    private int hashWalk = Animator.StringToHash("isWalk");

    //ref
    Coroutine moveToTargetRoutine;
    ChangeRagDoll myChange;
    NavMeshAgent nv;

    //enemy -> player directrion
    Vector3 toTargetDir = Vector3.zero;
    Quaternion idleDir = Quaternion.identity;

    //flag
    private bool isStart = false;
    //public bool nvEnableFlag = true;

    //Idle ref
    //private bool startTurn = false;

    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        maxHp = hp;
        myChange = GetComponentInParent<ChangeRagDoll>();
        nv = GetComponentInParent<NavMeshAgent>();
        toTargetDir = (new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f))).normalized;
        Quaternion toTargetRot = Quaternion.LookRotation(new Vector3(toTargetDir.x, transform.position.y, toTargetDir.z));
        //transform.rotation = Quaternion.Slerp(transform.rotation, toTargetRot, Time.deltaTime * rotSpeed);

        StartCoroutine(Idle());
    }

    private void OnEnable()
    {
        //nv.enabled = true;
        target = GameObject.FindWithTag("Player").transform;
        if (isStart)
        {
            moveToTargetRoutine = StartCoroutine(MoveToTarget());
            anim.applyRootMotion = false;
        }
        
        isStart = true;
    }


    private IEnumerator Idle()
    {
        
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= recognitionRange)
        {
            anim.applyRootMotion = false;
            anim.SetBool(hashFind, true);

            yield return new WaitForSeconds(0.75f);
            StartCoroutine(MoveToTarget());

        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Idle());
        }
    }

    private IEnumerator MoveToTarget()
    {
        //toTargetDir = (target.position - transform.position).normalized;
        //Quaternion toTargetRot = Quaternion.LookRotation(new Vector3(toTargetDir.x , 0, toTargetDir.z));

        //transform.rotation = Quaternion.Slerp(transform.rotation, toTargetRot, Time.deltaTime * rotSpeed);

        anim.SetBool(hashWalk, true);
        anim.SetFloat("moveSpeed", nv.velocity.magnitude);
  
        float distance = Vector3.Distance(target.position, transform.position);


        if (distance <= attackRange && !isAttack)
            StartCoroutine(Attack(target.gameObject));
        else
        {
            if (nv.enabled)
                nv.SetDestination(target.transform.position);

            //startTurn = true;
            yield return new WaitForSeconds(.5f);
            moveToTargetRoutine = StartCoroutine(MoveToTarget());

            if (!nv.enabled)
                nv.enabled = true;
        }
    }

    private IEnumerator Attack(GameObject hitPerson)
    {
        isAttack = true;

        int randomAttackPattern = Random.Range(0, 2);
        float distance = Vector3.Distance(target.position, transform.position);
        nv.velocity = Vector3.zero;

        if (randomAttackPattern == 0)
        {
            anim.SetBool(hashAttack1, true);
            yield return new WaitForSeconds(0.7f);
            if (hitPerson.CompareTag("Player"))
                PlayerManager.ApplyDamage(damage);
            //////////////////////////////////////////////////////////ML
            //else if (hitPerson.CompareTag("PlayerAgent"))
            //    AgentManager.ApplyDamage(damage);
        }
        else
        {
            anim.SetBool(hashAttack2, true);
            yield return new WaitForSeconds(0.7f);
            if (hitPerson.CompareTag("Player"))
                PlayerManager.ApplyDamage(damage);
            //////////////////////////////////////////////////////////ML
            //else if (hitPerson.CompareTag("PlayerAgent"))
            //    AgentManager.ApplyDamage(damage);
        }
        if (distance <= attackRange)
        {
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(Attack(target.gameObject));
        }
        else
        {
            anim.SetBool(hashAttack1, false);
            anim.SetBool(hashAttack2, false);
            isAttack = false;

            
            moveToTargetRoutine = StartCoroutine(MoveToTarget());
        }
    }

    public void ApplyDamage(int damage)
    {

        hp -= damage;
        //Debug.Log(hp);
        if (hp <= 0)
        {
            Die();
            //gameObject.SetActive(false);
            //hp = 100;

            //////////////////////////////////////////////////////////ML
            //TestPlayerAgent1.isKill = true;
        }
    }
    private void Die()
    {
        myChange.StartCoroutine(myChange.ChangeRagdoll());
    }

    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!isAttack)
                StartCoroutine(Attack(collision.gameObject));
        }
        //if (collision.gameObject.CompareTag("PlayerAgent"))
        //{
        //    if (!isAttack)
        //        StartCoroutine(Attack(collision.gameObject));
        //}
    }
}
