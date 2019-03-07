using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollDIeCtrl : MonoBehaviour
{
    public Rigidbody hipRigid;
    public Rigidbody spineRigid;
    public Rigidbody leftThighRigid;
    public Rigidbody rightThighRigid;
    public Rigidbody headRigid;

    public Rigidbody leftArmRigid;
    public Rigidbody rightArmRigid;
    public Rigidbody leftForeArmRigid;
    public Rigidbody rightForeArmRigid;

    public Rigidbody leftShinRigid;
    public Rigidbody rightShinRigid;

    private Transform targetTr;

    private void Awake()
    {
        targetTr = GameObject.FindWithTag("Player").transform;
    }

    private void OnEnable()
    {
        

        Vector3 AttackedDir = (transform.position - targetTr.position).normalized;
        //leftShinRigid.AddForce(AttackedDir * 5f);
    }

    
}
