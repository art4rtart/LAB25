using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
	public GameObject player;
	public Animator fadeAnim;
	public GameObject elevator;
	public UIManager uiManager;
	public string nextSceneName;

	public void CloseElevator()
	{
		player.transform.SetParent(elevator.transform);

		player.GetComponent<CharacterController>().enabled = false;
		fadeAnim.SetTrigger("SceneEnd");

		SceneManager.LoadScene(nextSceneName);
	}

	public void DoorOpen()
	{
		uiManager.isMissionComplete = true;
	}
}
