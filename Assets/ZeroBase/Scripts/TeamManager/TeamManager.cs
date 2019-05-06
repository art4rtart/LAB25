using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
	[Header("Default")]
	public GameObject player;
	public Animator animator;
	public Slider teamTimeCountSlider;
	public LayerMask teamMask;

	[Header("Range")]
	public float findRadius;
	public float sliderTimeCount = 3f;


	List<Transform> teamMate = new List<Transform>();
	bool collideWithTeamMate;
	bool showTeamMateMessage;
	bool timeOut;

	void Update()
    {
		Collider[] teamInRadius = Physics.OverlapSphere(player.transform.position, findRadius, teamMask);

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
				sliderTimeCount = 3f;
			}
			timeOut = false;
		}

		if (showTeamMateMessage)
		{
			Debug.Log(sliderTimeCount);
			sliderTimeCount -= Time.deltaTime;
			teamTimeCountSlider.value = sliderTimeCount * 0.33f;

			if (sliderTimeCount < 0)
			{
				animator.SetTrigger("Hide");
				sliderTimeCount = 3f;
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
			sliderTimeCount = 3f;
		}
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(player.transform.position, findRadius);
	}
}
