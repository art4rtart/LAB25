using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCtrl : MonoBehaviour
{
    public Transform pickingPoint;

    private void OnMouseDown()
    {
       GetComponent<Rigidbody>().useGravity = false;
       transform.SetParent(pickingPoint.transform);
    }

    private void OnMouseUp()
    {
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.SetParent(null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        transform.GetComponent<Rigidbody>().useGravity = true;
        transform.SetParent(null);
    }
}
