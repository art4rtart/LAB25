using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
	public GameObject bioGoggle;
	public GameObject ak47;
	public GameObject adrenaline;
	public GameObject vest;
	public GameObject health;

	public GameObject Gate;

	public bool openGateMission = true;

	public void OpenGate()
	{
		Debug.Log("Open the Gate");
		Gate.GetComponent<Animator>().SetTrigger("OpenAndClose");
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			Debug.Log("Enter");
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("Stay");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			Debug.Log("Exit");
		}
	}
}
