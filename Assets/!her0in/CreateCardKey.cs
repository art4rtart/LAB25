using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCardKey : MonoBehaviour
{
	public Transform[] positions;
	public GameObject cardKey;
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			SetPosition();
		}
	}
	public void SetPosition()
	{
		Transform cardKeyPosition = positions[Random.Range(0, positions.Length)];

		cardKey.transform.position = new Vector3(cardKeyPosition.position.x + Random.Range(-8f, 8f), 1f, cardKeyPosition.position.z + Random.Range(-8f, 8f));
	}
}
