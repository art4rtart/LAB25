using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class her0inEnemy : MonoBehaviour
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
    public bool isAttack;
    private Transform target;

    //anim
    private Animator anim;
   
    private int hashAttack1 = Animator.StringToHash("isAttack1");
    private int hashAttack2 = Animator.StringToHash("isAttack2");
    private int hashWalk = Animator.StringToHash("isWalk");

    //ref
    private Coroutine moveToTargetRoutine;
    private ChangeRagDoll myChange;
    private NavMeshAgent nv;
    public RagDollDIeCtrl myRagDollCtrl;

    [HideInInspector]
    public GameObject hitObject;
    [HideInInspector]
    public Vector3 hitPos;

    //enemy -> player directrion
    private Vector3 toTargetDir = Vector3.zero;
    private Quaternion idleDir = Quaternion.identity;

    //flag
    private bool isStart = false;

    //Idle ref
    //private bool startTurn = false;

    private void Awake()
    {
        target = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
		rgbd = GetComponent<Rigidbody>();
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
            anim.applyRootMotion = false;
            moveToTargetRoutine = StartCoroutine(MoveToTarget());
        }

        isStart = true;
    }

	public SpawnEffect spawnEffect;
    private IEnumerator Idle()
    {

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= recognitionRange)
        {
			if(this.transform.parent.name == "TInfectee")
				spawnEffect.enabled = true;

			anim.applyRootMotion = false;
            StartCoroutine(MoveToTarget());
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Idle());
        }
    }

    public IEnumerator MoveToTarget()
    {
		//toTargetDir = (target.position - transform.position).normalized;
		//Quaternion toTargetRot = Quaternion.LookRotation(new Vector3(toTargetDir.x , 0, toTargetDir.z));

		//transform.rotation = Quaternion.Slerp(transform.rotation, toTargetRot, Time.deltaTime * rotSpeed);

		anim.SetBool(hashWalk, true);
        anim.SetFloat("moveSpeed", 2);

        float distance = Vector3.Distance(target.position, transform.position);

		if (nv.enabled )
		{
            if(nv.remainingDistance < 30f && nv.remainingDistance != 0)
            {
                if (this.transform.parent.name == "TInfectee(Clone)")
                {
                    spawnEffect.enabled = true;
                }
            }
		}

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

	Rigidbody rgbd;

    private void Attack(GameObject hitPerson)
    {
		int randomAttackPattern = Random.Range(0, 2);
        hitObject = hitPerson;
        if (randomAttackPattern == 0)
            anim.SetBool(hashAttack1, true);
        else
            anim.SetBool(hashAttack2, true);
        anim.SetBool(hashWalk, false);

        StartCoroutine(AttackLoop(hitPerson));
    }

    private IEnumerator AttackLoop(GameObject hitPerson)
    {
        yield return new WaitForSeconds(1.5f);

        anim.SetBool(hashAttack1, false);
        anim.SetBool(hashAttack2, false);

        yield return new WaitForSeconds(1.5f);

        float distance = Vector3.Distance(hitPerson.transform.position, transform.position);

        if (distance <= attackRange && !isAttack)
            Attack(hitPerson);
        else
            moveToTargetRoutine = StartCoroutine(MoveToTarget());
    }

    public void ApplyDamage(int damage)
    {

        hp -= damage;
   
        if (hp <= 0)
        {
            myRagDollCtrl.speed = nv.velocity.magnitude;
            myRagDollCtrl.AttackedPos = hitPos;
            Die();
            //////////////////////////////////////////////////////////ML
            //isAttack = false;
            //transform.parent.gameObject.SetActive(false);
            //hp = 100;


            //Healing.isKill = true;
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
        if (collision.gameObject.CompareTag("PlayerAgent"))
        {
            if (!isAttack)
                Attack(collision.gameObject);
        }
    }
}
