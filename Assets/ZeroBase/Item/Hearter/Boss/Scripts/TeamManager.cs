using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
	public float findRadius;
	public LayerMask teamMask;

	bool collideWithTeamMate;
	bool showTeamMateMessage;
	bool timeOut;

	public Animator animator;
	public Slider teamTimeCountSlider;

	List<Transform> teamMate = new List<Transform>();

	public float timeCount = 3f;

    void Update()
    {
		Collider[] teamInRadius = Physics.OverlapSphere(transform.position, findRadius, teamMask);

		if (teamInRadius.Length == 0)
		{
			collideWithTeamMate = false;
		}

		else
		{
			for (int i = 0; i < teamInRadius.Length; i++)
			{
				if (teamInRadius[i].transform.CompareTag("Medic") || teamInRadius[i].transform.CompareTag("Guard"))
				{
					collideWithTeamMate = true;
				}
			}
		}

		if (collideWithTeamMate)
		{
			for (int i = 0; i < teamInRadius.Length; i++)
			{
				if (!showTeamMateMessage && !timeOut && !teamInRadius[i].GetComponent<TeamPlayer>().isMyTeam)
				{
					animator.SetTrigger("Show");
					showTeamMateMessage = true;
				}
			}
		}

		else
		{
			if (showTeamMateMessage)
			{
				animator.SetTrigger("Hide");
				showTeamMateMessage = false;
				timeCount = 3f;
			}
			timeOut = false;
		}

		if (showTeamMateMessage)
		{
			timeCount -= Time.deltaTime;
			teamTimeCountSlider.value = timeCount * 0.33f;

			if (timeCount < 0)
			{
				animator.SetTrigger("Hide");
				timeCount = 3f;
				showTeamMateMessage = false;
				timeOut = true;
			}
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			for (int i = 0; i < teamInRadius.Length; i++)
			{
				teamInRadius[i].GetComponent<TeamPlayer>().isMyTeam = true;
				StartCoroutine(teamInRadius[i].GetComponent<TeamPlayer>().FollowPlayer());
			}

			animator.SetTrigger("Hide");
			showTeamMateMessage = false;
			teamMate = null;
			timeOut = true;
			timeCount = 3f;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, findRadius);
	}
}
