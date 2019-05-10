using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentCtrl : MonoBehaviour
{
    Animator anim;
    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    bool lootAtPlayer = false;

    public Transform playerTr;
    private NavMeshAgent nv;

    private TeamCtrl myTeamCtrl;
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);

        anim = GetComponent<Animator>();
        nv = GetComponent<NavMeshAgent>();
        myTeamCtrl = GetComponent<TeamCtrl>();
        nv.updatePosition = false;

    }

    // Update is called once per frame
    void Update()
    {

        //nv.SetDestination(playerTr.position);

        Vector3 worldDeltaPosition = nv.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && (nv.remainingDistance + 0.8) - nv.stoppingDistance   > nv.radius;

       
        // Update animation parameters
        anim.SetBool("isRun", shouldMove);
        anim.SetFloat("velx", velocity.x);
        anim.SetFloat("vely", velocity.y);


        if (worldDeltaPosition.magnitude > nv.radius)
            transform.position = nv.nextPosition - 0.9f * worldDeltaPosition;

    }

    void OnAnimatorMove()
    {
        // Update position based on animation movement using navigation surface height
        Vector3 position = anim.rootPosition;
        position.y = nv.nextPosition.y;
        transform.position = position;
    }
}
