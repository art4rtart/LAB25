using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
	public bool openGateMission = true;

	public void OpenAndCloseGate(GameObject Gate)
	{
		Gate.GetComponent<Animator>().SetTrigger("OpenAndClose");
	}

	public void OpenGate(GameObject Gate)
	{
		Gate.GetComponent<Animator>().SetBool("GateOpen", true);
	}

	//void OnTriggerEnter(Collider other)
	//{
	//	if(other.gameObject.CompareTag("Player"))
	//	{
	//		Debug.Log("Enter");
	//	}
	//}

	//void OnTriggerStay(Collider other)
	//{
	//	if (other.gameObject.CompareTag("Player"))
	//	{
	//		Debug.Log("Stay");
	//	}
	//}

	//void OnTriggerExit(Collider other)
	//{
	//	if (other.gameObject.CompareTag("Player"))
	//	{
	//		Debug.Log("Exit");
	//	}
	//}
}
