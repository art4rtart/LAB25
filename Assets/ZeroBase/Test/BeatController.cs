using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour
{
	float moveSpeed = 10f;

	void FixedUpdate()
	{
		transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
	}
}
