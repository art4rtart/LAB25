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
    [HideInInspector]
    public float speed;
    public Vector3 AttackedPos;
    private void Awake()
    {
        targetTr = GameObject.FindWithTag("Player").transform;
    }

    private void OnEnable()
    {
        Vector3 AttackedDir = (transform.position - targetTr.position).normalized;
        AttackedDir = (AttackedDir + transform.forward * speed).normalized;


        if (AttackedPos.y <= 0.6)
        {
            if (AttackedPos.x <= 0)
            {
                Debug.Log("LeftShit");
                leftShinRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("RightShit");
                rightShinRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
            }
        }
        else if (AttackedPos.y <= 1)
        {
            if (AttackedPos.x <= 0)
            {
                Debug.Log("LeftThi");
                leftThighRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("RightThi");
                rightThighRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
            }
        }
        else if (AttackedPos.y <= 1.3)
        {
            if (AttackedPos.x <= -0.1)
            {
                Debug.Log("LeftFore");
                leftForeArmRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
            }
            else if (AttackedPos.x >= 0.1)
            {
                Debug.Log("RightFore");
                rightForeArmRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("Hip");
                hipRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
            }
        }
        else if (AttackedPos.y <= 1.6)
        {
            if (AttackedPos.x <= -0.1)
            {
                Debug.Log("LeftArm");
                leftArmRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
            }
            else if (AttackedPos.x >= 0.1)
            {
                Debug.Log("RightArm");
                rightArmRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
            }
            else
            {
                Debug.Log("Spine");
                spineRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
            }
        }
        else if (AttackedPos.y > 1.6)
        {
            Debug.Log("Head");
            headRigid.AddForce(AttackedDir * 150f, ForceMode.Impulse);
        }
    }


}
