using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
	void Awake()
	{
		this.gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, Random.Range(0f, 360f), gameObject.transform.eulerAngles.z);
	}
}
