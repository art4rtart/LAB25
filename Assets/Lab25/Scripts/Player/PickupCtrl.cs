using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCtrl : MonoBehaviour
{
    public Transform pickingPoint;

    private void OnMouseDown()
    {
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		transform.SetParent(pickingPoint.transform);
		this.gameObject.layer = 26;
	}

	private void OnMouseUp()
    {
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		transform.GetComponent<Rigidbody>().useGravity = true;
        transform.SetParent(null);
		this.gameObject.layer = 13;
	}
}
