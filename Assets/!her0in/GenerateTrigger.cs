using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTrigger : MonoBehaviour
{
	public InfecteeGenerator generator;

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			StartCoroutine(generator.Generate());
		}
	}
}
