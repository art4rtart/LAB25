using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFinder : MonoBehaviour
{
	public Transform target;
	private NavMeshPath path;
	private float elapsed = 0.0f;

	[HideInInspector]
	public List<Vector3> destinations = new List<Vector3>();
	[HideInInspector]
	public int destinationsIndex = 0;
	[HideInInspector]
	public int maxIndex = 0;

	public GameObject pathObject;

	void OnEnable()
	{
		path = new NavMeshPath();
		elapsed = 0.0f;

		NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
		for (int i = 0; i < path.corners.Length - 1; i++)
		{
			destinations.Add(path.corners[i]);
			if (Vector3.Distance(path.corners[i], path.corners[i + 1]) >= 5f)
			{
                Vector3 middle = path.corners[i] + ((path.corners[i + 1] - path.corners[i]) * 0.5f);
				destinations.Add(middle);
			}
		}

		destinations.Add(path.corners[path.corners.Length-1]);
		maxIndex = destinations.Count;

		//for(int i =0; i <destinations.Count; i++)
		//	Debug.Log(destinations[i]);

		pathObject.SetActive(true);
	}

	public float refreshRate = 1.0f;

	void Update()
	{
		elapsed += Time.deltaTime;

		if (elapsed > refreshRate)
		{
			elapsed -= refreshRate;
			NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);

			destinations.Clear();
			maxIndex = 0;
			destinationsIndex = 0;

			for (int i = 0; i < path.corners.Length - 1; i++)
			{
				destinations.Add(path.corners[i]);
				if (Vector3.Distance(path.corners[i], path.corners[i + 1]) >= 5f)
				{
					Vector3 middle = path.corners[i] + ((path.corners[i + 1] - path.corners[i]) * 0.5f);

					destinations.Add(middle);
				}
			}

			destinations.Add(path.corners[path.corners.Length - 1]);
			maxIndex = destinations.Count;
			pathObject.transform.position = destinations[destinationsIndex] + Vector3.up * 1f;
		}

		for (int i = 0; i < path.corners.Length - 1; i++)
			Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
	}
}
