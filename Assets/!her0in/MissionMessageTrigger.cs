using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionMessageTrigger : MonoBehaviour
{
	MissionScripts missionScript;

	void Awake()
	{
		missionScript = FindObjectOfType<MissionScripts>().GetComponent<MissionScripts>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			missionScript.GetComponent<Animator>().SetTrigger("Finish");
			missionScript.Type();
			this.gameObject.SetActive(false);
		}
	}
}
