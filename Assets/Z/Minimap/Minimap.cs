using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
	public GameObject outline;
	public float outlineRotateSpeed = 10f;


	void Update()
	{
		outline.transform.Rotate(Vector3.back * outlineRotateSpeed * Time.deltaTime);
	}
}
