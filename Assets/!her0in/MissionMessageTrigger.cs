using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionMessageTrigger : MonoBehaviour
{
	MissionScripts missionScript;
	public bool setToDefault;

	void Awake()
	{
		missionScript = FindObjectOfType<MissionScripts>().GetComponent<MissionScripts>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			missionScript.GetComponent<Animator>().ResetTrigger("Finish");
			if (setToDefault) missionScript.GetComponent<Animator>().SetTrigger("Finish");
			missionScript.Type();
			this.gameObject.SetActive(false);
		}
	}
}
