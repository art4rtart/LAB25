using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
	public float rotateSpeed;
	public float moveSpeed;

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(this.transform.position.z <= 4.5)
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

		transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
    }
}
