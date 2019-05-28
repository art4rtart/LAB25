using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDetect : MonoBehaviour
{
	public PathFinder pathFinder;

	void Start()
	{
		this.gameObject.transform.position = pathFinder.destinations[pathFinder.destinationsIndex] + Vector3.up * 1f;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("agent"))
		{
			if (pathFinder.destinationsIndex < pathFinder.maxIndex - 1)
			{
				pathFinder.destinationsIndex++;
				this.gameObject.transform.position = pathFinder.destinations[pathFinder.destinationsIndex] + Vector3.up * 1f;
			}
			else
			{
				this.gameObject.SetActive(false);
				other.gameObject.GetComponent<Her0inAgent>().agentRunSpeed = 0;
			}
		}
	}
}
