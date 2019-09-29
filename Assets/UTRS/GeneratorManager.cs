using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{
	public GameObject[] Generator;
	int index = 0;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if (index > Generator.Length - 1) return;
			StartCoroutine(Generator[index].GetComponent<InfecteeGenerator>().Generate());
			index++;
		}
	}
}
