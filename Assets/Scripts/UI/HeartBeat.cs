using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
	public float speed = 5;
	public float waitTime = .3f;
	public Transform pathHolder;
	public GameObject beat;
	public GameObject beat2;
	float timeCount = 0;
	float timeCount2 = 0;
	bool gogo = false;

	void Start()
	{
		Vector3[] waypoints = new Vector3[pathHolder.childCount];
		for(int i = 0; i < waypoints.Length; i++)
		{
			waypoints[i] = pathHolder.GetChild(i).position;
		}

		StartCoroutine(FollowPath(waypoints));
	}

	IEnumerator FollowPath(Vector3[] waypoints)
	{
		beat.transform.position = waypoints[0];
		beat2.transform.position = waypoints[0];

		int targetWaypointIndex = 1;
		int targetWaypointIndex2 = 1;

		Vector3 targetWaypoint = waypoints[targetWaypointIndex];
		Vector3 targetWaypoint2 = waypoints[targetWaypointIndex2];

		beat.transform.LookAt(targetWaypoint);
		beat2.transform.LookAt(targetWaypoint2);

		while (true)
		{
			beat.transform.position = Vector3.MoveTowards(beat.transform.position, targetWaypoint, speed * Time.deltaTime);

			if (targetWaypointIndex == waypoints.Length - 1)
			{
				timeCount += Time.deltaTime;
				if (timeCount > waitTime)
				{
					targetWaypointIndex = 0;
					targetWaypoint = waypoints[targetWaypointIndex];
					beat.transform.position = waypoints[targetWaypointIndex];
					beat.GetComponent<TrailRenderer>().Clear();
					timeCount = 0;
				}
			}

			else
			{
				if (beat.transform.position == targetWaypoint)
				{
					targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
					targetWaypoint = waypoints[targetWaypointIndex];
				}
			}

			if (targetWaypointIndex == 15)
				gogo = true;

			if (gogo)
			{
				beat2.transform.position = Vector3.MoveTowards(beat2.transform.position, targetWaypoint2, speed * Time.deltaTime);
				if (targetWaypointIndex2 == waypoints.Length - 1)
				{
					timeCount2 += Time.deltaTime;
					if (timeCount2 > waitTime)
					{
						targetWaypointIndex2 = 0;
						targetWaypoint2 = waypoints[targetWaypointIndex2];
						beat2.transform.position = waypoints[targetWaypointIndex2];
						beat2.GetComponent<TrailRenderer>().Clear();
						timeCount2 = 0;
					}
				}
				else
				{
					if (beat2.transform.position == targetWaypoint2)
					{
						targetWaypointIndex2 = (targetWaypointIndex2 + 1) % waypoints.Length;
						targetWaypoint2 = waypoints[targetWaypointIndex2];
					}
				}
			}

			yield return null;
		}
	}
}
