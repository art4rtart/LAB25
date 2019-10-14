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
	string[] loadingNames = { "Loading1", "Loading2", "Loading3", "Loading4", "Loading5" };

	void Awake()
	{
		player = FindObjectOfType<PlayerCtrl>().gameObject;
	}

	public void CloseElevator()
	{
		player.transform.SetParent(elevator.transform);

		player.GetComponent<CharacterController>().enabled = false;
		fadeAnim.SetTrigger("SceneEnd");

		LevelLoader.sceneName = nextSceneName;

		StartCoroutine(Timer());
	}

	public void DoorOpen()
	{
		uiManager.isMissionComplete = true;
	}

	IEnumerator Timer()
	{
		yield return new WaitForSeconds(2f);

		SceneManager.LoadScene(nextSceneName);
	}
}
