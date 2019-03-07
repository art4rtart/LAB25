using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfecteeCtrl : MonoBehaviour
{
    //infectee attributes
    public int hp;
    public int maxHp;
    public int damage;
    public float speed;
    public int rotSpeed;
    public float attackRange;
    public float recognitionRange;
    

    //functional attributes
    private bool isAttack;
    private Transform target;

    //anim
    private Animator anim;
    private int hashFind = Animator.StringToHash("isFind");
    private int hashAttack = Animator.StringToHash("isAttack");
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
        toTargetDir = (new Vector3(Random.Range(-1.0f,1.0f), 0, Random.Range(-1.0f, 1.0f))).normalized;
        Quaternion toTargetRot = Quaternion.LookRotation(new Vector3(toTargetDir.x, transform.position.y, toTargetDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, toTargetRot, Time.deltaTime * rotSpeed);

        StartCoroutine(Idle());
    }

    private void OnEnable()
    {
        //nv.enabled = true;
        target = GameObject.FindWithTag("Player").transform;
        if(isStart)
            moveToTargetRoutine = StartCoroutine(MoveToTarget());
        anim.applyRootMotion = false;
        isStart = true;
    }

    private void OnDisable()
    {
        //nv.enabled = false;
        //Debug.Log("Disable");
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    if (nvEnableFlag)
        //    {
        //        nvEnableFlag = false;
        //        nv.enabled = false;
        //    }
        //    else
        //    {
        //        nvEnableFlag = true;
        //        nv.enabled = true;
        //    }
        //}
        //anim.SetBool(hashWalk, false);
        //if( !startTurn )
        //    transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private IEnumerator Idle()
    {
        //startTurn = false;
        //float distance = Vector3.Distance(target.position, transform.position);
        //anim.SetBool(hashWalk, true);

        //if (distance <= recognitionRange)
        //{
        //    StartCoroutine(MoveToTarget());
        //    speed = 2;
        //    yield break;
        //}

        //speed = 1f;
        //float randomTime = Random.Range(2, 5);
        //yield return new WaitForSeconds(randomTime);
        //anim.SetBool(hashWalk, false);

        //toTargetDir = (new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f))).normalized;
        //Quaternion toTargetRot = Quaternion.LookRotation(new Vector3(toTargetDir.x, transform.position.y, toTargetDir.z));
        //transform.rotation = Quaternion.Slerp(transform.rotation, toTargetRot, Time.deltaTime * rotSpeed);
        //startTurn = true;
        //yield return new WaitForSeconds(1.0f);

        //StartCoroutine(Idle());

        //speed = 0.25f;
        anim.applyRootMotion = true;
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= recognitionRange)
        {
            StartCoroutine(MoveToTarget());
            anim.applyRootMotion = false;
            anim.SetBool(hashFind, true);
            speed = 0f;
            yield return new WaitForSeconds(3.0f);
            speed = 2;
        }
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(Idle());
    }

    private IEnumerator MoveToTarget()
    {
        //toTargetDir = (target.position - transform.position).normalized;
        //Quaternion toTargetRot = Quaternion.LookRotation(new Vector3(toTargetDir.x , 0, toTargetDir.z));

        //transform.rotation = Quaternion.Slerp(transform.rotation, toTargetRot, Time.deltaTime * rotSpeed);

        anim.SetBool(hashWalk, true);
        
        float distance = Vector3.Distance(target.position, transform.position);


        if (distance <= attackRange && !isAttack)
            StartCoroutine(Attack(target.gameObject));
      

        if ( nv.enabled )
            nv.SetDestination(target.transform.position);

        //startTurn = true;
        yield return new WaitForSeconds(.5f);
        moveToTargetRoutine = StartCoroutine(MoveToTarget());

        if (!nv.enabled)
            nv.enabled = true;
    }

    private IEnumerator Attack(GameObject hitPerson)
    {
        isAttack = true;
        anim.SetBool(hashAttack, true);
        yield return new WaitForSeconds(0.7f);
        if ( hitPerson.CompareTag("Player"))
            PlayerManager.ApplyDamage(damage);

        yield return new WaitForSeconds(0.3f);
        isAttack = false;
        anim.SetBool(hashAttack, false);
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
        }
    }
    private void Die()
    {
        myChange.StartCoroutine(myChange.ChangeRagdoll());
    }

    public void OnCollisionStay(Collision collision)
    {
        //if( collision.gameObject.tag == "Player")
        //{
        //    if (!isAttack)
        //        StartCoroutine(Attack(collision.gameObject));
        //}
        if (collision.gameObject.CompareTag("PlayerAgent"))
        {
            if (!isAttack)
                StartCoroutine(Attack(collision.gameObject));
        }
    }
}
