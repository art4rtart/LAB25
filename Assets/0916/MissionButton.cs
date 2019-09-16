using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionButton : MonoBehaviour
{
	void OnCollisionEnter(Collision other)
	{
		if(other.gameObject.CompareTag("Moveable"))
		{
			Debug.Log("HI");
			Debug.Log(other.gameObject.name);
			StartCoroutine(Checker());
		}
	}

	IEnumerator Checker()
	{
		float time = 0;

		while(true)
		{
			time += Time.deltaTime;

			if(time > 1f)
			{
				break;
			}

			yield return null;
		}

		Debug.Log("Open");
	}
}
