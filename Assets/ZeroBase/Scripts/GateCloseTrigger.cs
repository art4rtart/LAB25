using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCloseTrigger : MonoBehaviour
{
	public string gateType;
	public Animator gateAnimator;
	public GameObject[] deactivateSectors;
	public GameObject[] activateSectors;

	public SetMaterialValue setMaterialValue;

	bool closed = false;

	UIManager uiManager;

	void Awake()
	{
		uiManager = FindObjectOfType<UIManager>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && !closed)
		{
			if (uiManager.isMissionStart)
				uiManager.isMissionComplete = true;

			if (gateType == "Dynamic")
			{
				gateAnimator.SetTrigger("Close");
			}

			else
			{
				gateAnimator.SetBool("GateOpen", false);
			}

			for (int i = 0; i < deactivateSectors.Length; i++)
				deactivateSectors[i].SetActive(false);

			for (int i = 0; i < activateSectors.Length; i++)
				activateSectors[i].SetActive(true);

			setMaterialValue.DefaultTrigger();

			closed = true;
		}
	}
}
