using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TInfecteeCtrl : MonoBehaviour
{
    //infectee attributes
    public int hp;
    public int maxHp;
    public int damage;
    public float attackRange;
    public float recognitionRange;

    //functional attributes
    public bool isAttack;
    private Transform target;

    //anim
    private Animator anim;
    private int hashAttack = Animator.StringToHash("isAttack");
    private int hashRun = Animator.StringToHash("isRun");

    //ref
    private Coroutine moveToTargetRoutine;
    private ChangeTRagDoll myChange;
    private NavMeshAgent nv;

    //enemy -> player directrion
    private Vector3 toTargetDir = Vector3.zero;

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
        myChange = GetComponentInParent<ChangeTRagDoll>();
        nv = GetComponentInParent<NavMeshAgent>();
        toTargetDir = (new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f))).normalized;
        

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
            StartCoroutine(MoveToTarget());
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Idle());
        }
    }

    public IEnumerator MoveToTarget()
    {
        anim.SetBool(hashRun, true);
        

        float distance = Vector3.Distance(target.position, transform.position);


        if (distance <= attackRange && !isAttack)
            Attack(target.gameObject);
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

    private void Attack(GameObject hitPerson)
    {
        anim.SetBool(hashAttack, true);
        

        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(0.5f);

        anim.SetBool(hashAttack, false);
        anim.SetBool(hashRun, false);

        yield return new WaitForSeconds(1.5f);

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= attackRange && !isAttack)
            Attack(target.gameObject);
        else
            moveToTargetRoutine = StartCoroutine(MoveToTarget());
    }

    public void ApplyDamage(int damage)
    {

        hp -= damage;

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
                Attack(collision.gameObject);
        }
        //if (collision.gameObject.CompareTag("PlayerAgent"))
        //{
        //    if (!isAttack)
        //        StartCoroutine(Attack(collision.gameObject));
        //}
    }
}
