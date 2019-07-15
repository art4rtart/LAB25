using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfecteeGirlCtrl : MonoBehaviour
{
    public float recognizeRange;
    public Transform target;
    public SkinnedMeshRenderer mySkin;
    public bool isAttacked = false;

    private bool wasBoom = false;
    public GameObject boomParticle;
    Health info;

    [HideInInspector]
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        info = GetComponent<Health>();
        info.damaged.AddListener(SetAttackTrigger);
    }

    // Update is called once per frame
    void Update()
    {
        if (wasBoom)
            return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= recognizeRange || isAttacked)
        {
            anim.SetBool("isBoom", true);
            wasBoom = true;
            Invoke("Boom", 3.5f);
        }
    }

    public void ChangeSkinColor()
    {
        mySkin.material.color = mySkin.material.color+ new Color(0.01f, 0, 0, 0);
        //Debug.Log(mySkin.material.color);
    }

    public void SetAttackTrigger(Vector3 tmp)
    {
        isAttacked = true;
    }

    void Boom()
    {
        info.died.Invoke();
        Instantiate(boomParticle, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);
    }
}
