using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Infectee5Ctrl : MonoBehaviour
{
    public Animator anim;

    public NavMeshAgent nv;
    private readonly string arriveStr = "isArrived";

    public Transform[] destinations;
    private Transform finalDes;
    private int secondRoar = 0;

    // Start is called before the first frame update
    void Start()
    {
        finalDes = destinations[0];
        StartCoroutine(Move());
    }

    void CheckArrived()
    {
        //Debug.Log("Check");
        if (secondRoar.Equals(0))
        {
            if (nv.remainingDistance < 0.5f)
            {
                anim.SetBool(arriveStr, true);
                StartCoroutine(Roar());
            }
            else
            {
                //Debug.Log("false1");
                anim.SetBool("isArrived", false);
                StartCoroutine(Move());
            }
        }
        else
        {
            //Debug.Log(nv.remainingDistance);
            if (!nv.remainingDistance.Equals(0))
            {
                if (nv.remainingDistance >= 0.5f)
                {
                    //Debug.Log("ASD");
                    anim.SetBool("isArrived", false);
                    StartCoroutine(Move());
                }
                else
                {
                    //Debug.Log("ZXC");
                    Destroy(this.gameObject);
                }
            }
            else
                StartCoroutine(Move());
        }


        //yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Move()
    {
        nv.SetDestination(finalDes.position);
        yield return new WaitForSeconds(0.2f);

        CheckArrived();
    }

    IEnumerator Roar()
    {
        secondRoar++;
        finalDes = destinations[1];
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("isArrived", false);
        nv.SetDestination(finalDes.position);
        CheckArrived();


    }
}
